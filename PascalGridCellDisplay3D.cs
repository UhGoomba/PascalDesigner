using Godot;
using System;
using PascalDesigner;

public partial class PascalGridCellDisplay3D : TextDisplay3D
{
    private PascalGridCell _gridCell;
    private NinePatchRect _ninePatchRect;

    public override void _Ready()
    {
        base._Ready();

        _ninePatchRect = GetNode<NinePatchRect>("SubViewport/Control/MarginContainer/NinePatchRect");
        SetSelected(false);
    }
    
    public void SetGridCell(PascalGridCell gridCell)
    {
        _gridCell = gridCell;
    }

    public void SetSelected(bool selected)
    {
        _ninePatchRect.Modulate = (selected ? new Color("ffffff") : new Color("000000"));
    }
}
