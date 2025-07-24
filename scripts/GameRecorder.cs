using Godot;
using System;
using System.Collections.Generic;

public partial class GameRecorder : Node
{
	public static void SaveGameRecord(List<(int, int)> moves)
	{
		if (!DirAccess.DirExistsAbsolute("res://game_records"))
		{
			GD.Print("Dir doesn't exist");
		}
		using var file = FileAccess.Open("res://game_records/game_record.dat", FileAccess.ModeFlags.Write);
		string serializedMoves = "";
		foreach ((int, int) move in moves)
		{
			serializedMoves += $"({move.Item1},{move.Item2}) ";
		}
		GD.Print(serializedMoves);
		file.StoreString(serializedMoves);
	}
}
