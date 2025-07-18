using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

public partial class Game : Node
{
	[Export] private Board _board;
	private Client _client;
	private GameLogic _logic;

	public override void _Ready()
	{
		_client = new Client();
		_logic = new GameLogic(_client, _board);
		AddChild(_client);
		AddChild(_logic);
	}


}
