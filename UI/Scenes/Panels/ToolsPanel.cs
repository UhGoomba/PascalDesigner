using Godot;
using System;
using PascalDesigner;

public partial class ToolsPanel : MenuPanel
{
	const int NewFixedCellValue = 1;
	private void On_AddFixedCell()
	{
		PascalGrid grid = UIVars.PascalGenerator.Grid;

		int w = 0;
		while (grid.HasCellAtPosition(new JamisonianCoordinate(0, 0, 0, w)))
		{
			w++;
		}

		PascalGridCell cell = new PascalGridCellFixed(NewFixedCellValue, new JamisonianCoordinate(0, 0, 0, w));
		grid.AddCell(cell);
		UIVars.PascalGenerator.RenderCell(cell);
	}
}
