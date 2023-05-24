using Godot;
using System;
using PascalDesigner;

public partial class PascalGridCellDisplay3D : TextDisplay3D
{
    private PascalGridCell _gridCell;
    private PascalGrid _grid;
    private NinePatchRect _ninePatchRect;
    private bool _isActive = false;
    private bool _isFixed = false;
    private bool _isSelected = false;

    public override void _Ready()
    {
        base._Ready();

        _ninePatchRect = GetNode<NinePatchRect>("SubViewport/Control/MarginContainer/NinePatchRect");
    }

    private void OnSelectedInput(bool multi)
    {
        SetSelected(GetNode<Selection>("/root/Selection").Select(this, multi));
    }

    public void SetGridCell(PascalGridCell gridCell, PascalGrid grid)
    {
        _gridCell = gridCell;
        _grid = grid;
        _isFixed = _gridCell is PascalGridCellFixed;
        
        SetSelected(_isSelected);
        SetText(_gridCell.Value.ToString());
        GlobalPosition = PascalGenerator.JamisonianToRectangular(_gridCell.Position).coordinate;
    }

    public void SetSelected(bool selected, bool fancy = true)
    {
        _isSelected = selected;
        _ninePatchRect.Modulate = !_isFixed ? (selected ? new Color("ffffff") : new Color("000000")) : (selected ? new Color("b3d1ff") : new Color("002966"));
    }

    public void SetPosition(JamisonianCoordinate position)
    {
        _grid.MoveCell(_gridCell, position);
        GlobalPosition = PascalGenerator.JamisonianToRectangular(_gridCell.Position).coordinate;
    }

    public void SetValue(int value)
    {
        _gridCell.SetValue(value, true);
        SetText(_gridCell.Value.ToString());
    }

    public void Delete()
    {
        _grid.RemoveCell(_gridCell);
        SetActive(false);
        GetNode<Selection>("/root/Selection").ValidateSelected();
        Free(); // TODO - USE OBJECT POOLING 2
    }
    
    public PascalGridCell GetCell()
    {
        return _gridCell;
    }
    public bool IsActive() { return _isActive; }
    public void SetActive(bool isActive) { this._isActive = isActive; }
}
