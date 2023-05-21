using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PascalDesigner;

public enum ComputeOperator
{
	Addition,
	SubtractionBT,
	SubtractionTB,
	Multiplication,
	DivisionBT,
	DivisionTB,
	ExponentialBT,
	ExponentialTB
}
public class PascalGrid
{
	public event EventHandler OnComputeFinished;

	private List<PascalGridCellFixed> _fixedCells = new List<PascalGridCellFixed>();
	private List<PascalGridLine> _lines = new List<PascalGridLine>();

	private Queue<int> _linesLeftInCompute;

	public PascalGrid()
	{
		
	}
	
	public void PrepareCompute(int lineRange)
	{
		_linesLeftInCompute = new Queue<int>();

		foreach (PascalGridCellFixed fixedCell in _fixedCells.OrderByDescending(cell => cell.Position.W)) // TODO - CHANGE METHOD OF GETTINGS LINES TO COMPUTE
		{
			for (int i = 0; i < lineRange; i++)
			{
				if(!_linesLeftInCompute.Contains(fixedCell.Position.W - i)) _linesLeftInCompute.Enqueue(fixedCell.Position.W - i);
			}
		}
	}
	public void ComputeLine()
	{ 
		if (!_linesLeftInCompute.Any()) return;

		int lineW = _linesLeftInCompute.Dequeue();
		PascalGridLine line = _lines.FirstOrDefault(line => line.W == lineW); // This could be using: "AddOrGetLine(lineW - 1);", however I want this to throw an error if it messes up, because then something worse is wrong
		PascalGridLine lineBelow = AddOrGetLine(lineW - 1);
		if (line == null) GD.PushError("Line did not contain data during computing, this SHOULD NOT be happening.");
			
		foreach (PascalGridCell cell in line.Cells)
		{
			cell.SetValue(cell.GetParentPositions().Select(pascalCell => GetCellAtPosition(pascalCell)).Select(pascalCell => pascalCell.Value).Sum()); // TODO - Make this work with multiple types of equations

			JamisonianCoordinate[] childrenPositions = cell.GetChildrenPositions();
			
			if (cell.Children == null)
			{
				cell.Children = new PascalGridCell[3];
				
				for (int i = 0; i < cell.Children.Length; i++)
				{
					cell.Children[i] = lineBelow.Cells.FirstOrDefault(pascalCell => pascalCell.Position.Equals(childrenPositions[i]));
					if (cell.Children[i] == null)
					{
						cell.Children[i] = GetCellAtPosition(childrenPositions[i]);
						AddCell(cell.Children[i]);
					}
				}
			}
		}

		if (!_linesLeftInCompute.Any())
		{
			foreach (PascalGridCell cell in lineBelow.Cells)
			{
				cell.SetValue(cell.GetParentPositions().Select(pascalCell => GetCellAtPosition(pascalCell)).Select(pascalCell => pascalCell.Value).Sum());
			}

			if (OnComputeFinished != null) OnComputeFinished(this, EventArgs.Empty);
		}
	}
	public void FinishCompute()
	{
		while (_linesLeftInCompute.Any())
		{
			ComputeLine();
		}
	}

	public PascalGridLine AddOrGetLine(int w)
	{
		PascalGridLine line = _lines.FirstOrDefault(line => line.W == w);
		if (line == null)
		{
			line = new PascalGridLine(w);
			_lines.Add(line);
		}

		return line;
	}
	
	public void AddCell(PascalGridCell cell)
	{
		if (cell is PascalGridCellFixed)
		{
			_fixedCells.Add(cell as PascalGridCellFixed);
		}

		PascalGridLine line = _lines.FirstOrDefault(line => line.W == cell.Position.W);
		if (line == null)
		{
			line = new PascalGridLine(cell.Position.W);
			_lines.Add(line);
		}
		line.Cells.Add(cell);
	}

	public (List<PascalGridCellFixed> fixedCells, List<PascalGridLine> lines) GetRenderData()
	{
		return (fixedCells: _fixedCells, lines: _lines);
	}
	
	public PascalGridCell GetCellAtPosition(JamisonianCoordinate position)
	{
		PascalGridLine line = _lines.FirstOrDefault(line => line.W == position.W);

		if (line != null)
		{
			PascalGridCell foundCell = line.Cells.FirstOrDefault(cell => cell.Position.Equals(position));
			if (foundCell != null) return foundCell;
		}

		return new PascalGridCell(0, position); // TODO - Change hardcoded background value
	}
	
	public int GetValueAtPosition(JamisonianCoordinate position)
	{
		return GetCellAtPosition(position).Value;
	}
	
}
public class PascalGridLine
{
	public List<PascalGridCell> Cells;
	public int W;

	public PascalGridLine(int w)
	{
		Cells = new List<PascalGridCell>();
		W = w;
	}
}

public class PascalGridCell
{
	public int Value { get; private set; }
	public PascalGridCell[] Children;
	public readonly JamisonianCoordinate Position;
	
	public PascalGridCell(int value, JamisonianCoordinate position)
	{
		Value = value;
		Position = position;
	}
	
	public JamisonianCoordinate[] GetChildrenOffsets()
	{
		return new JamisonianCoordinate[] { new JamisonianCoordinate(0,0,0,-1), new JamisonianCoordinate(-1,1,0,-1), new JamisonianCoordinate(0,1,-1,-1)};
	}

	public JamisonianCoordinate[] GetChildrenPositions()
	{
		return GetChildrenOffsets().Select(x => Position + x).ToArray();
	}
	
	public JamisonianCoordinate[] GetParentPositions()
	{
		return GetChildrenOffsets().Select(x => Position - x).ToArray();
	}

	public virtual void SetValue(int value)
	{
		Value = value;
	}
}

public class PascalGridCellFixed : PascalGridCell
{
	public ComputeOperator ComputeOperator { get; private set; }
	
	public PascalGridCellFixed(int value, JamisonianCoordinate position, ComputeOperator computeOperator = ComputeOperator.Addition) : base(value, position)
	{
		ComputeOperator = computeOperator;
	}

	public override void SetValue(int value)
	{
		return;
	}
}

public class JamisonianCoordinate //lol
{
	public readonly int X,Y,Z,W;

	public JamisonianCoordinate(Vector3I triangularCoordinate, int height)
	{
		X = triangularCoordinate.X;
		Y = triangularCoordinate.Y;
		Z = triangularCoordinate.Z;
		W = height;
	}

	public JamisonianCoordinate(int x, int y, int z, int w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	#region Operators

	public static JamisonianCoordinate operator +(JamisonianCoordinate a) => a;
	public static JamisonianCoordinate operator -(JamisonianCoordinate a) => new JamisonianCoordinate(-a.X,-a.Y,-a.Z,-a.W);
	
	public static JamisonianCoordinate operator +(JamisonianCoordinate a, JamisonianCoordinate b)
		=> new JamisonianCoordinate(a.X + b.X, a.Y + b.Y, a.Z + b.Z,a.W + b.W);
	public static JamisonianCoordinate operator -(JamisonianCoordinate a, JamisonianCoordinate b)
		=> a + (-b);
	
	public static bool operator ==(JamisonianCoordinate lhs, JamisonianCoordinate rhs)
	{
		if (lhs is null)
		{
			if (rhs is null)
			{
				return true;
			}

			// Only the left side is null.
			return false;
		}
		// Equals handles case of null on right side.
		return lhs.Equals(rhs);
	}

	public static bool operator !=(JamisonianCoordinate lhs, JamisonianCoordinate rhs) => !(lhs == rhs);
	
	public override bool Equals ( object obj )
	{
		return Equals(obj as JamisonianCoordinate);
	}
	public bool Equals ( JamisonianCoordinate obj )
	{
		return obj != null && obj.X == this.X && obj.Y == this.Y && obj.Z == this.Z && obj.W == this.W;
	}

	public override int GetHashCode() => (X, Y, Z, W).GetHashCode();
	
	#endregion

	public Vector3I GetTriangularCoordinate()
	{
		return new Vector3I(X, Y, Z);
	}
}