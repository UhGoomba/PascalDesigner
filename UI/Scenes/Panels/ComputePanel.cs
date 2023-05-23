using Godot;
using System;

public partial class ComputePanel : MenuPanel
{
	public override void _Ready()
	{
		base._Ready();

		UIVars.LinesToCompute = Mathf.RoundToInt(GetNode<SpinBox>("HBoxContainer/VBoxContainer/Content/MarginContainer/VBoxContainer/HBoxContainer/SpinBox").Value);
	}

	private void On_ComputeButtonPressed()
	{
		UIVars.PascalGenerator.Compute();
	}
	
	private void On_ClearValuesButtonPressed()
	{
		UIVars.PascalGenerator.ClearAllValues();
	}

	private void On_LinesValueChanged(float value)
	{
		UIVars.LinesToCompute = Mathf.RoundToInt(value);
	}
}
