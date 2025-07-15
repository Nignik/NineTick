using Godot;
using System;

public partial class Game : Node
{
	[Export] Texture2D whiteStone;
	[Export] Texture2D blackStone;

	public override void _Ready()
	{
		foreach (Node node in GetTree().GetNodesInGroup("tiles"))
		{
			if (node is Tile tile)
			{
				tile.Clicked += OnObjectClicked;
			}
		}
	}

	private void OnObjectClicked(Tile tile)
	{
		GD.Print($"Object was clicked: {tile.Name}");
		tile.SetTexture(blackStone);
	}
}
