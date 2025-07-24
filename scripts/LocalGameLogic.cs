using Godot;
using System;
using System.Xml;

public partial class LocalGameLogic : GameLogic
{
    private PlayerColor[] _playerColors = [PlayerColor.White, PlayerColor.Black];
    private int _activePlayer = 0;

    public LocalGameLogic(Board board)
        : base(board)
    {
        _playerColor = PlayerColor.White;
    }

    public override void OnTileClicked(Tile tile)
    {
        int blockRow = tile.row / 3;
		int blockCol = tile.col / 3;
        if (_activeBlock == (blockRow, blockCol) || _activeBlock == (-1, -1))
        {
            Move(tile.row, tile.col, _playerColor);
            ProcessMove();

            int activeBlockRow = tile.row % 3;
            int activeBlockCol = tile.col % 3;
            _activeBlock = (activeBlockRow, activeBlockCol);
            _activePlayer = (_activePlayer + 1) % 2;
            _playerColor = _playerColors[_activePlayer];
		}
    }
}
