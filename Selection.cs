using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Selection : Node
{
	public event EventHandler OnSelectionChanged;
	
	private List<PascalGridCellDisplay3D> _selectedCells;

	public Selection()
	{
		_selectedCells = new List<PascalGridCellDisplay3D>();
	}

	public bool Select(PascalGridCellDisplay3D input, bool multi)
	{
		if (OnSelectionChanged != null) OnSelectionChanged(this, EventArgs.Empty);

		if (!multi) // Single Select
		{
			_selectedCells.ForEach(cell => cell.SetSelected(false));
			_selectedCells.Clear();
			
			_selectedCells.Add(input);

			return true;
		}
		else
		{
			if (_selectedCells.Contains(input))
			{
				_selectedCells.Remove(input);
				return false;
			}
			else
			{
				_selectedCells.Add(input);
				return true;
			}
		}
	}

	public void ValidateSelected()
	{
		_selectedCells = _selectedCells.Where(cell => cell != null && cell.IsActive()).ToList();
		_selectedCells.ForEach(cell => cell.SetSelected(true, false));
	}
}
