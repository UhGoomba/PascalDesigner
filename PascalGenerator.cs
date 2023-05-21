using Godot;
using System;
using PascalDesigner;

public partial class PascalGenerator : Node3D
{
	private PascalGrid _generator = new PascalGrid();

	public override void _Ready()
	{
		_generator.AddCell(new PascalGridCellFixed(1,new JamisonianCoordinate(0,0,0,0)));
		_generator.PrepareCompute(7);
		_generator.FinishCompute();

		var renderData = _generator.GetRenderData();
		var textScene = GD.Load<PackedScene>("res://text_display_3d.tscn");
		foreach (PascalGridLine line in renderData.lines)
		{
			foreach (PascalGridCell cell in line.Cells)
			{
				TextDisplay3D text = textScene.Instantiate<TextDisplay3D>();
				AddChild(text);
				text.SetText(cell.Value.ToString());
				text.GlobalPosition = JamisonianToRectangular(cell.Position).coordinate;
			}
		}
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
