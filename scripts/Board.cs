using Godot;
using System;
using Godot.Collections;
using System.Linq.Expressions;

public partial class Board : Node
{
    [Export] public PackedScene TileScene { get; set; }

    private const int _width = 9, _height = 9;
    private Tile[,] _tiles;

    private PlayerColor[,] _tileColors;
    private PlayerColor[,] _blockColors;

    public override void _Ready()
    {
        _tiles = new Tile[_height, _width];

        for (int r = 0; r < _height; r++)
        {
            for (int c = 0; c < _width; c++)
            {
                _tiles[r, c] = TileScene.Instantiate<Tile>();
                float offset = _tiles[r, c].GetSizeInPixels();
                _tiles[r, c].Position = new Vector2(c * offset, r * offset);
                _tiles[r, c].Initialize(r, c);

                AddChild(_tiles[r, c]);
            }
        }

        _tileColors = new PlayerColor[_height, _width];
        for (int r = 0; r < _height; r++)
            for (int c = 0; c < _width; c++)
                _tileColors[r, c] = PlayerColor.None;


        _blockColors = new PlayerColor[3, 3];
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                _blockColors[r, c] = PlayerColor.None;
    }

    public Tile[,] GetTiles()
    {
        return _tiles;
    }

    public PlayerColor[,] GetBlock(int row, int col)
    {
        int r0 = row * 3;
        int c0 = col * 3;

        var block = new PlayerColor[3, 3];
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                block[r, c] = _tileColors[r0 + r, c0 + c];

        return block;
    }

    public PlayerColor[,] GetBlockView()
    {
        return _blockColors;
    }

    public void PaintTile(int row, int col, PlayerColor color)
    {
        _tileColors[row, col] = color;
        _tiles[row, col].SetColor(color);
    }

    public void PaintBlock(int row, int col, PlayerColor color)
    {
        _blockColors[row, col] = color;
    }
}
