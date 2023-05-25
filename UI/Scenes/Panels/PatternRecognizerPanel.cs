using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PascalDesigner;

public partial class PatternRecognizerPanel : MenuPanel
{
	static readonly HttpClient client = new HttpClient();
	
	[ExportGroup("Sub Panels")]
	[Export] public Control TwoSelectedPanel { get; set; }
	[Export] public Control OtherSelectedPanel { get; set; }
	
	[ExportGroup("Sub Panel UI")]
	[Export] public Label SequenceLabel { get; set; }
	
	private Selection _selection;

	public override void _Ready()
	{
		base._Ready();
		
		_selection = GetNode<Selection>("/root/Selection");
		_selection.OnSelectionChanged += OnSelectionChanged;
		UpdatePanel();
	}

	private void OnSelectionChanged(object sender, EventArgs e)
	{
		UpdatePanel();
	}

	private void UpdatePanel()
	{
		TwoSelectedPanel.Visible = false;
		OtherSelectedPanel.Visible = false;

		if (_selection.GetSelectedCount() == 2)
		{
			TwoSelectedPanel.Visible = true;
			String text = "Sequence: ";
			_selection.GetSelected().ForEach(cell => text = text + cell.GetCell().Value.ToString() + ", ");
			SequenceLabel.Text = text;
		}
		else
		{
			OtherSelectedPanel.Visible = true;
		}
	}
	
	private void On_DetectPattern()
	{
		List<PascalGridCell> pattern = _selection.GetSelected().Select(cell => cell.GetCell()).ToList();
		JamisonianCoordinate offset = pattern[0].Position - pattern[1].Position;

		while (UIVars.PascalGenerator.Grid.HasCellAtPosition(pattern[pattern.Count() - 1].Position + offset))
		{
			pattern.Add(UIVars.PascalGenerator.Grid.GetCellAtPosition(pattern[pattern.Count() - 1].Position + offset));
		}
		
		String text = "https://oeis.org/search?q=";
		pattern.Select(cell => cell.Value).ToList().ForEach(value => text = text + value.ToString() + ",");
		text = text + "&fmt=json";
	}
}
