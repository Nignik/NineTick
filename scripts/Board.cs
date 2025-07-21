using Godot;
using System;
using Godot.Collections;
using System.Linq.Expressions;

public partial class Board : Node2D
{
    [Export] public PackedScene TileScene { get; set; }

    private const int _gap = 3;
    private const int _size = 9;
    private Tile[,] _tiles;

    private PlayerColor[,] _tileColors;
    private PlayerColor[,] _blockColors;

    public override void _Ready()
    {
        // Initialize the tiles
        _tiles = new Tile[_size, _size];
        for (int r = 0; r < _size; r++)
        {
            for (int c = 0; c < _size; c++)
            {
                _tiles[r, c] = TileScene.Instantiate<Tile>();
                float offset = _tiles[r, c].GetSizeInPixels();
                _tiles[r, c].Position = new Vector2(
                    c * offset + (c/3) * _gap,
                    r * offset + (r/3) * _gap
                );
                _tiles[r, c].Initialize(r, c);

                AddChild(_tiles[r, c]);
            }
        }

        // Position the board
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;
        Tile last = _tiles[_size - 1, _size - 1];
        float pixelSize = last.Position.X * Scale.X + last.GetSizeInPixels();
        Position = (screenSize - new Vector2(pixelSize, pixelSize)) * 0.5f;

        // Color the tiles
        _tileColors = new PlayerColor[_size, _size];
        for (int r = 0; r < _size; r++)
            for (int c = 0; c < _size; c++)
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
