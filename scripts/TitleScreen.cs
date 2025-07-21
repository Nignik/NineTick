using Godot;
using System;

public partial class TitleScreen : Node
{
    [Export] private PackedScene _gameScene;
    [Export] private Button _startButton;
    [Export] private Button _quitButton;

    public override void _Ready()
    {
        _startButton.Pressed += OnStartPressed;
        _quitButton.Pressed += () => GetTree().Quit();
    }

    private void OnStartPressed()
    {
        GetTree().ChangeSceneToPacked(_gameScene);
    }
}
