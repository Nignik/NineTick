using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

public partial class Game : Node
{
	[Export] private Board _board;
	[Export] private bool _online;
	private Client _client;
	private GameLogic _logic;

	public override void _Ready()
	{
		if (_online)
		{
			_client = new Client();
			_logic = new OnlineGameLogic(_client, _board);
			AddChild(_client);
		}
		else
		{
			_logic = new LocalGameLogic(_board);
		}

		AddChild(_logic);
	}


}
