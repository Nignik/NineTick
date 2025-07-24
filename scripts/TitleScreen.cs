using Godot;
using System;

public partial class TitleScreen : Node
{
    [Export] private PackedScene _onlineGameScene;
    [Export] private PackedScene _localGameScene;
    [Export] private Button _playOnlineButton;
    [Export] private Button _playLocallyButton;
    [Export] private Button _quitButton;

    public override void _Ready()
    {
        _playOnlineButton.Pressed += OnPlayPlayOnlineClicked;
        _playLocallyButton.Pressed += OnPlayLocallyClicked;
        _quitButton.Pressed += () => GetTree().Quit();
    }

    private void OnPlayPlayOnlineClicked()
    {
        GetTree().ChangeSceneToPacked(_onlineGameScene);
    }

    private void OnPlayLocallyClicked()
    {
        GetTree().ChangeSceneToPacked(_localGameScene);
    }
}
