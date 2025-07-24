using Godot;
using System;

public partial class OnlineGameLogic : GameLogic
{
    private Client _client;
    private bool _isMoving = false;
    private string _playerId = "";

    public OnlineGameLogic(Client client, Board board)
        : base(board)
    {
        _client = client;
    }

    public override void _Ready()
    {
        base._Ready();
        _client.PlayerJoinApprovedSignal += OnPlayerJoinApproved;
        _client.PlayerMoveSignal += OnPlayerMove;
    }


    public void OnPlayerJoinApproved(string playerId, int playerColor)
    {
        _playerColor = (PlayerColor)playerColor;
        if (_playerColor == PlayerColor.White)
            _isMoving = true;
    }

    private void OnPlayerMove(int row, int col, int color)
    {
        Move(row, col, (PlayerColor)color);
		ProcessMove();

		int blockRow = row % 3;
		int blockCol = col % 3;
		_activeBlock = (blockRow, blockCol);

        if ((PlayerColor)color != _playerColor)
            _isMoving = true;
    }

    public override void OnTileClicked(Tile tile)
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

