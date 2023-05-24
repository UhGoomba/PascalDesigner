using Godot;
using System;

public partial class TextDisplaySelectionCollision : StaticBody3D
{
	[Signal]
	public delegate void SelectedInputEventHandler(bool multi);

	public void Select(bool multi)
	{
		EmitSignal("SelectedInput", multi);
	}
}
