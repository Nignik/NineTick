using Godot;
using System;

public partial class Tile : Area2D
{
	[Signal] public delegate void ClickedEventHandler(Tile tile);
	[Export] Sprite2D sprite;
	public override void _Ready()
	{
		AddToGroup("tiles");
		InputEvent += OnInputEvent;
	}

	public void SetTexture(Texture2D newTexture)
	{
		sprite.Texture = newTexture;
	}

	private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			EmitSignal(SignalName.Clicked, this);
			GD.Print($"Signal emitted");
		}
	}
	

}
