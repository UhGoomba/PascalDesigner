using Godot;
using System;
using PascalDesigner;

public partial class PascalGridCellDisplay3D : TextDisplay3D
{
    private PascalGridCell _gridCell;

    public void SetGridCell(PascalGridCell gridCell)
    {
        _gridCell = gridCell;
    }
}
