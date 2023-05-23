using Godot;
using System;

public partial class MenuPanel : VBoxContainer
{
    protected UIVars UIVars;
    
    public override void _Ready()
    {
        base._Ready();
        
        UIVars = GetNode<UIVars>("/root/UIVars");
    }
}
