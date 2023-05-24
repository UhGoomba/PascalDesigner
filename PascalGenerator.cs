using Godot;
using System;
using System.Collections.Generic;
using PascalDesigner;

public partial class PascalGenerator : Node3D
{
	private PascalGrid _grid = new PascalGrid();
	private UIVars _uiVars;
	private Selection _selection;
	private List<PascalGridCellDisplay3D> _cellDisplays = new List<PascalGridCellDisplay3D>();

	public override void _Ready()
	{
		_uiVars = GetNode<UIVars>("/root/UIVars");
		_selection = GetNode<Selection>("/root/Selection");
		_uiVars.PascalGenerator = this;
		_grid.AddCell(new PascalGridCellFixed(1,new JamisonianCoordinate(0,0,0,0)));
	}

	public void Compute()
	{
		ClearAllValues();
		_grid.SetBackgroundValue(_uiVars.BackgroundValue);
		_grid.SetTriangleCellStartingValue(_uiVars.TriangleCellStartingValue);
		_grid.SetComputeOperator(_uiVars.Operator);
		_grid.PrepareCompute(_uiVars.LinesToCompute);
		_grid.FinishCompute();

		var renderData = _grid.GetRenderData();
		var textScene = GD.Load<PackedScene>("res://text_display_3d.tscn");
		foreach (PascalGridLine line in renderData.lines)
		{
			foreach (PascalGridCell cell in line.Cells)
			{
				PascalGridCellDisplay3D display = textScene.Instantiate() as PascalGridCellDisplay3D;
				AddChild(display);
				display.SetText(cell.Value.ToString());
				display.SetGridCell(cell);
				display.GlobalPosition = JamisonianToRectangular(cell.Position).coordinate;
				display.SetActive(true);
				_cellDisplays.Add(display);
			}
		}
		_selection.ValidateSelected();
	}

	public void ClearAllValues()
	{
		foreach (PascalGridCellDisplay3D textDisplay3D in _cellDisplays)
		{
			textDisplay3D.SetActive(false);
			textDisplay3D.Free(); // TODO - Object Pooling
		}
		
		_cellDisplays.Clear();
		_grid.ClearValues();
		_selection.ValidateSelected();
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
