using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;



public partial class Game : Node
{
	[Export] private Board _board;

	private bool _isMoving = true;
	private PlayerColor _playerColor = PlayerColor.White;

	private static readonly (int r, int c)[][] WinLines = {
		// Rows
		[(0,0),(0,1),(0,2)],
		[(1,0),(1,1),(1,2)],
		[(2,0),(2,1),(2,2)],
		// Cols
		[(0,0),(1,0),(2,0)],
		[(0,1),(1,1),(2,1)],
		[(0,2),(1,2),(2,2)],
		// Diagonals
		[(0,0),(1,1),(2,2)],
		[(0,2),(1,1),(2,0)],
	};

	public override void _Ready()
	{
		foreach (Tile tile in _board.GetTiles())
			tile.Clicked += OnTileClicked;
	}


	public override void _Process(double delta)
	{
		PlayerColor[,] boardState = _board.GetBlockView();

		for (int r = 0; r < 3; r++)
		{
			for (int c = 0; c < 3; c++)
			{
				PlayerColor color = CheckBlock(_board.GetBlock(r, c));
				if (color != PlayerColor.None && boardState[r, c] == PlayerColor.None)
				{
					_board.PaintBlock(r, c, color);
					GD.Print($"Block {r}, {c} has been captured.");
				}
			}
		}

		PlayerColor boardColor = CheckBlock(boardState);
		if (boardColor != PlayerColor.None)
			GD.Print("Game eneded");
	}

	public void SetMoving(bool isMoving)
	{
		_isMoving = isMoving;
	}

	private PlayerColor CheckBlock(PlayerColor[,] block)
	{
		foreach (var line in WinLines)
		{
			PlayerColor first = block[line[0].r, line[0].c];
			if (first == PlayerColor.None)
				continue;

			bool allMatch = true;
			for (int i = 1; i < line.Length; i++)
			{
				if (block[line[i].r, line[i].c] != first)
				{
					allMatch = false;
					break;
				}
			}
			if (allMatch)
				return first;
		}

		for (int r = 0; r < 3; r++)
			for (int c = 0; c < 3; c++)
				if (block[r, c] == PlayerColor.None)
					return PlayerColor.None;

		return PlayerColor.Mix;
	}

	private void OnTileClicked(Tile tile)
	{
		if (_isMoving)
		{
			_board.PaintTile(tile.row, tile.col, _playerColor);
		}
	}
}
