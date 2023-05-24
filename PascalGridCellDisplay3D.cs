using Godot;
using System;
using PascalDesigner;

public partial class PascalGridCellDisplay3D : TextDisplay3D
{
    private PascalGridCell _gridCell;
    private NinePatchRect _ninePatchRect;
    private bool _isActive = false;

    public override void _Ready()
    {
        base._Ready();

        _ninePatchRect = GetNode<NinePatchRect>("SubViewport/Control/MarginContainer/NinePatchRect");
        SetSelected(false);
    }

    private void OnSelectedInput(bool multi)
    {
        SetSelected(GetNode<Selection>("/root/Selection").Select(this, multi));
    }

    public void SetGridCell(PascalGridCell gridCell)
    {
        _gridCell = gridCell;
    }

    public PascalGridCell GetCell()
    {
        return _gridCell;
    }

    public void SetSelected(bool selected, bool fancy = true)
    {
        _ninePatchRect.Modulate = (selected ? new Color("ffffff") : new Color("000000"));
    }

    public bool IsActive() { return _isActive; }
    public void SetActive(bool isActive) { this._isActive = isActive; }
}
