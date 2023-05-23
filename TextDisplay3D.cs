using Godot;
using System;

public partial class TextDisplay3D : Node3D
{
	private Label _label;

	public void SetText(String text)
	{
		if(_label == null) _label = GetNode("SubViewport/Control/MarginContainer/MarginContainer/CenterContainer/Label") as Label;
		_label.Text = text;
	}
}
