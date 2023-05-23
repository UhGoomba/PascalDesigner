using Godot;
using System;
using System.Collections.Generic;
using PascalDesigner;

public partial class PascalGenerator : Node3D
{
	private PascalGrid _grid = new PascalGrid();
	private List<TextDisplay3D> textDisplays = new List<TextDisplay3D>();

	public override void _Ready()
	{
		GetNode<UIVars>("/root/UIVars").PascalGenerator = this;
		_grid.AddCell(new PascalGridCellFixed(1,new JamisonianCoordinate(0,0,0,0)));
	}

	public void Compute(int linesToCompute, int backgroundValue, ComputeOperator computeOperator)
	{
		ClearAllValues();
		_grid.SetBackgroundValue(backgroundValue);
		_grid.PrepareCompute(linesToCompute, computeOperator);
		_grid.FinishCompute();

		var renderData = _grid.GetRenderData();
		var textScene = GD.Load<PackedScene>("res://text_display_3d.tscn");
		foreach (PascalGridLine line in renderData.lines)
		{
			foreach (PascalGridCell cell in line.Cells)
			{
				PascalGridCellDisplay3D text = textScene.Instantiate<PascalGridCellDisplay3D>();
				AddChild(text);
				text.SetText(cell.Value.ToString());
				text.SetGridCell(cell);
				text.GlobalPosition = JamisonianToRectangular(cell.Position).coordinate;
				textDisplays.Add(text);
			}
		}
	}

	public void ClearAllValues()
	{
		foreach (TextDisplay3D textDisplay3D in textDisplays)
		{
			textDisplay3D.QueueFree();
		}
		
		textDisplays.Clear();
		_grid.ClearValues();
	}
	
	private const float J2RHORIZONTALX = 0.5f; // Just the way the wind blows
	private const float J2RHORIZONTALZ = 0.144337f; // should figure the math behind this number
	private const float J2RVERTICALZ = -0.866025403784f; // -Sqrt(3) / 2
	private const float J2RHEIGHTOFFSETY = 0.866025403784f; // Sqrt(3) / 2
	private const float J2RHEIGHTOFFSETZ = -0.57735f; // should figure the math behind this number
	public static (Vector3 coordinate, bool flipped) JamisonianToRectangular(JamisonianCoordinate coordinate)
	{
		Vector3 rectangular = (new Vector3(J2RHORIZONTALX, 0, J2RHORIZONTALZ) * (float)coordinate.X) +
							  (new Vector3(0, 0, J2RVERTICALZ) * (float)coordinate.Y) +
							  (new Vector3(-J2RHORIZONTALX, 0, J2RHORIZONTALZ) * (float)coordinate.Z);
		rectangular += new Vector3(0,J2RHEIGHTOFFSETY,J2RHEIGHTOFFSETZ) * coordinate.W; 
		
		return (
			coordinate: rectangular,
			flipped: (coordinate.X + coordinate.Y + coordinate.Z != 0)
		);
	}
}
