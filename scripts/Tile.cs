using Godot;
using System;

public partial class Tile : Area2D
{

	public int row, col;

	[Export] private Sprite2D _sprite;
	[Export] private Texture2D _whiteStone;
	[Export] private Texture2D _blackStone;

	[Signal] public delegate void ClickedEventHandler(Tile tile);

	public void Initialize(int row, int col)
	{
		this.row = row;
		this.col = col;
	}

	public override void _Ready()
	{
		AddToGroup("tiles");
		InputEvent += OnInputEvent;
	}

	public float GetSizeInPixels()
	{
		return _sprite.Texture.GetSize().X * _sprite.Scale.X;
	}

	public void SetColor(PlayerColor color)
	{
		switch (color)
		{
			case PlayerColor.White:
				_sprite.Texture = _whiteStone;
				break;
			case PlayerColor.Black:
				_sprite.Texture = _blackStone;
				break;
		}
	}

	private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			EmitSignal(SignalName.Clicked, this);
		}
	}
}
