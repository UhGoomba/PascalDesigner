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

    public override void _Input(InputEvent inputEvent)
    {
        base._Input(inputEvent);
        if (inputEvent is InputEventMouseButton && ((InputEventMouseButton)inputEvent).Pressed)
        {
            if (((InputEventMouseButton)inputEvent).ButtonIndex == MouseButton.Left)
            {
                
            }
        }
    }

    private void On_CollisionInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 shape, int shapeIdx)
    {
        if (inputEvent is InputEventMouseButton)
        {
            if ((inputEvent as InputEventMouseButton).ButtonIndex == MouseButton.Left &&
                (inputEvent as InputEventMouseButton).Pressed)
            {
                GD.Print("test2");
            }
        }
        GD.Print("test");
    }
}
