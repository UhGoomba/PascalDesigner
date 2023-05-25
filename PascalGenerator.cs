using Godot;
using System;
using System.Collections.Generic;
using PascalDesigner;

public partial class PascalGenerator : Node3D
{
	public PascalGrid Grid { get; private set; }
	private UIVars _uiVars;
	private Selection _selection;
	private List<PascalGridCellDisplay3D> _cellDisplays = new List<PascalGridCellDisplay3D>();

	public override void _Ready()
	{
		Grid = new PascalGrid();;
		_uiVars = GetNode<UIVars>("/root/UIVars");
		_selection = GetNode<Selection>("/root/Selection");
		_uiVars.PascalGenerator = this;
		Grid.AddCell(new PascalGridCellFixed(1,new JamisonianCoordinate(0,0,0,0)));
	}

	public void Compute()
	{
		ClearAllValues();
		Grid.SetBackgroundValue(_uiVars.BackgroundValue);
		Grid.SetTriangleCellStartingValue(_uiVars.TriangleCellStartingValue);
		Grid.SetComputeOperator(_uiVars.Operator);
		Grid.PrepareCompute(_uiVars.LinesToCompute);
		Grid.FinishCompute();

		var renderData = Grid.GetRenderData();
		renderData.lines.ForEach(line => line.Cells.ForEach(RenderCell));
		_selection.ValidateSelected();
	}

	public void RenderCell(PascalGridCell cell)
	{
		var textScene = GD.Load<PackedScene>("res://text_display_3d.tscn");
		PascalGridCellDisplay3D display = textScene.Instantiate() as PascalGridCellDisplay3D;
		AddChild(display);
		display.SetGridCell(cell, Grid);
		display.SetActive(true);
		_cellDisplays.Add(display);
	}
	
	public void ClearAllValues()
	{
		foreach (PascalGridCellDisplay3D textDisplay3D in _cellDisplays)
		{
			if (!IsInstanceValid(textDisplay3D)) continue;
			textDisplay3D.SetActive(false);
			textDisplay3D.Free(); // TODO - Object Pooling
		}
		
		_cellDisplays.Clear();
		Grid.ClearValues();
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
