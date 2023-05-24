using Godot;
using System;
using PascalDesigner;

public partial class InspectorPanel : MenuPanel
{
    [ExportGroup("Sub Panels")]
    [Export] public Control NoSelectedPanel { get; set; }
    [Export] public Control OneSelectedPanel { get; set; }
    [Export] public Control TooManySelectedPanel { get; set; }
    [Export] public Control FixedCellSelectedPanel { get; set; }
    
    [ExportGroup("Sub Panel UI")]
    // Normal
    [Export] public Label CellSelectedPosition { get; set; }
    [Export] public Label CellSelectedValue { get; set; }
    // Fixed
    [Export] public SpinBox CellFixedSelectedPositionX { get; set; }
    [Export] public SpinBox CellFixedSelectedPositionY { get; set; }
    [Export] public SpinBox CellFixedSelectedPositionZ { get; set; }
    [Export] public SpinBox CellFixedSelectedPositionW { get; set; }
    [Export] public SpinBox CellFixedSelectedValue { get; set; }
    
    private Selection _selection;

    private PascalGridCellDisplay3D _selectedCellDisplay;
    private PascalGridCell _selectedCell;
    private PascalGridCellFixed _selectedFixedCell;

    private JamisonianCoordinate _newFixedPosition = new JamisonianCoordinate(0,0,0,0);
    
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
        NoSelectedPanel.Visible = false;
        OneSelectedPanel.Visible = false;
        TooManySelectedPanel.Visible = false;
        FixedCellSelectedPanel.Visible = false;
        _selectedCellDisplay = null;
        _selectedCell = null;
        _selectedFixedCell = null;
        
        if (_selection.GetSelectedCount() <= 0) NoSelectedPanel.Visible = true;
        else if (_selection.GetSelectedCount() == 1)
        {
            _selectedCellDisplay = _selection.GetFirstSelected();
            _selectedCell = _selectedCellDisplay.GetCell();
            if (_selectedCell is PascalGridCellFixed)
            {
                _selectedFixedCell = _selectedCell as PascalGridCellFixed;
                if (_selectedFixedCell == null) return;
                
                FixedCellSelectedPanel.Visible = true;
                _newFixedPosition = _selectedFixedCell.Position;
                FixedCellUpdateValues();
            }
            else
            {
                OneSelectedPanel.Visible = true;

                CellSelectedPosition.Text = "Position: " + _selectedCell.Position.ToString();
                CellSelectedValue.Text = "Value: " + _selectedCell.Value.ToString();
            }
        }
        else if (_selection.GetSelectedCount() > 1) TooManySelectedPanel.Visible = true;
    }

    private void FixedCellUpdateValues()
    {
        CellFixedSelectedPositionX.Value = _selectedFixedCell.Position.X;
        CellFixedSelectedPositionY.Value = _selectedFixedCell.Position.Y;
        CellFixedSelectedPositionZ.Value = _selectedFixedCell.Position.Z;
        CellFixedSelectedPositionW.Value = _selectedFixedCell.Position.W;
        CellFixedSelectedValue.Value = _selectedFixedCell.Value;
    }
    
    private void On_FixedPositionChange(float value, int id)
    {
        if (_selectedFixedCell == null) return;

        if (id == 0) _newFixedPosition = new JamisonianCoordinate(Mathf.RoundToInt(value), _newFixedPosition.Y, _newFixedPosition.Z, _newFixedPosition.W);
        if (id == 1) _newFixedPosition = new JamisonianCoordinate(_newFixedPosition.X, Mathf.RoundToInt(value), _newFixedPosition.Z, _newFixedPosition.W);
        if (id == 2) _newFixedPosition = new JamisonianCoordinate(_newFixedPosition.X, _newFixedPosition.Y, Mathf.RoundToInt(value), _newFixedPosition.W);
        if (id == 3) _newFixedPosition = new JamisonianCoordinate(_newFixedPosition.X, _newFixedPosition.Y, _newFixedPosition.Z, Mathf.RoundToInt(value));
    }

    private void On_FixedPositionSet()
    {
        int sum = _newFixedPosition.X + _newFixedPosition.Y + _newFixedPosition.Z;
        GD.Print(sum);
        GD.Print(_newFixedPosition.W % 2);
        if (_newFixedPosition.W % 2 != sum)
        {
            FixedCellUpdateValues();
            return;
        }
        
        _selectedCellDisplay.SetPosition(_newFixedPosition);
    }
    
    private void On_FixedValueChange(float value)
    {
        if (_selectedFixedCell == null) return;

        _selectedCellDisplay.SetValue(Mathf.RoundToInt(value));
    }
    
    private void On_FixedValueDelete()
    {
        if (_selectedFixedCell == null) return;

        _selectedCellDisplay.Delete();
    }
}
