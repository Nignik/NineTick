using Godot;
using System;

public partial class GameLogic : Node
{
	private Client _client;
	private Board _board;
	private (int, int) _activeBlock = (-1, -1);

	private bool _isMoving = false;
	private string _playerId = "";
	private PlayerColor _playerColor = PlayerColor.None;

	private static readonly (int r, int c)[][] WinLines = [
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
	];

	public GameLogic(Client client, Board board)
	{
		_client = client;
		_board = board;
	}

	public override void _Ready()
	{
		foreach (Tile tile in _board.GetTiles())
			tile.Clicked += OnTileClicked;

		_client.PlayerJoinApprovedSignal += OnPlayerJoinApproved;
		_client.PlayerMoveSignal += OnPlayerMove;
	}

	public void ProcessMove()
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
					_activeBlock = (-1, -1);
					GD.Print($"Block {r}, {c} has been captured.");
				}
			}
		}

		PlayerColor boardColor = CheckBlock(boardState);
		if (boardColor != PlayerColor.None)
			GD.Print("Game eneded");
	}

	private void Move(int row, int col, PlayerColor color)
	{
		_board.PaintTile(row, col, color);
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

	private void OnPlayerJoinApproved(string playerId, int playerColor)
	{
		_playerColor = (PlayerColor)playerColor;
		if (_playerColor == PlayerColor.White)
			_isMoving = true;
	}

	private void OnPlayerMove(int row, int col, int color)
	{
		Move(row, col, (PlayerColor)color);
		ProcessMove();
		if ((PlayerColor)color != _playerColor)
			_isMoving = true;

		int blockRow = row % 3;
		int blockCol = col % 3;
		_activeBlock = (blockRow, blockCol);
	}

	private void OnTileClicked(Tile tile)
	{
		int blockRow = tile.row / 3;
		int blockCol = tile.col / 3;
		if (_isMoving && (_activeBlock == (blockRow, blockCol) || _activeBlock == (-1, -1)))
		{
			Move(tile.row, tile.col, _playerColor);
			_isMoving = false;
			NetworkMessage msg = MessageFactory.CreatePlayerMoveMessage(_playerId, _playerColor, tile.row, tile.col);
			_client.SendMessage(msg);
		}
	}
}
