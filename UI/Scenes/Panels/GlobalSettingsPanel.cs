using Godot;
using System;
using PascalDesigner;

public partial class GlobalSettingsPanel : MenuPanel
{
	public override void _Ready()
	{
		base._Ready();
		
		UIVars.BackgroundValue = Mathf.RoundToInt(GetNode<SpinBox>("HBoxContainer/VBoxContainer/Content/MarginContainer/VBoxContainer/BackgroundNumber/SpinBox").Value);
		UIVars.TriangleCellStartingValue = Mathf.RoundToInt(GetNode<SpinBox>("HBoxContainer/VBoxContainer/Content/MarginContainer/VBoxContainer/TriangleCellStartingNumber/SpinBox").Value);
		var optionButton = GetNode<OptionButton>("HBoxContainer/VBoxContainer/Content/MarginContainer/VBoxContainer/OperationType/OptionButton");
		UIVars.Operator = (ComputeOperator)optionButton.GetSelectedId();
	}
	
	private void On_BackgroundValueValueChanged(float value)
	{
		UIVars.BackgroundValue = Mathf.RoundToInt(value);
	}
	
	private void On_TriangleCellStartingValueValueChanged(float value)
	{
		UIVars.TriangleCellStartingValue = Mathf.RoundToInt(value);
	}

	private void On_OperationOptionButtonItemSelected(int index)
	{
		UIVars.Operator = (ComputeOperator)index;
	}
}
