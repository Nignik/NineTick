using Godot;
using System;

public partial class GameLogic : Node
{
    private Client _client;
    private Board _board;

    private bool _isMoving = true;
    private string _playerId = "";
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

    public GameLogic(Client client, Board board)
    {
        _client = client;
        _board = board;
    }

    public override void _Ready()
    {
        foreach (Tile tile in _board.GetTiles())
            tile.Clicked += OnTileClicked;

        _client.PlayerMoveSignal += OnPlayerMove;
    }

    public void Move(int row, int col, PlayerColor color)
    {
        _board.PaintTile(row, col, color);
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

    private void OnPlayerMove(int row, int col, int color)
    {
        Move(row, col, (PlayerColor)color);
        SetMoving(true);
    }

    private void OnTileClicked(Tile tile)
    {
        if (_isMoving)
        {
            Move(tile.row, tile.col, _playerColor);
            NetworkMessage msg = MessageFactory.CreatePlayerMoveMessage(_playerId, _playerColor, tile.row, tile.col);
            _client.SendMessage(msg);
            _isMoving = false;
        }
    }
}
