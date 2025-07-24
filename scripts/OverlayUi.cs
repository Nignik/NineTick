using Godot;
using System;

public partial class OverlayUi : CanvasLayer
{
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("show_overlay_ui"))
        {
            Visible = !Visible;
        }
    }
}
