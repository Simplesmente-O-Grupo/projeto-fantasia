using System;
using Godot;
using Godot.Collections;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.GUI;
public partial class Leaderboard : Control
{
	[Signal]
	public delegate void MenuRequestedEventHandler();
	private static readonly PackedScene LeaderboardItemScene = GD.Load<PackedScene>("res://scenes/GUI/leaderboard_item.tscn");

	private VBoxContainer leaderboard;
	private Button BackButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		leaderboard = GetNode<VBoxContainer>("VBoxContainer/VBoxContainer");
		BackButton = GetNode<Button>("VBoxContainer/exit");

		BackButton.Pressed += () => EmitSignal(SignalName.MenuRequested);

		bool hasLeaderboardFile = FileAccess.FileExists("user://placar.json");
		if (hasLeaderboardFile)
		{
			using var leaderboardFile = FileAccess.Open("user://placar.json", FileAccess.ModeFlags.Read);
			string boardString = leaderboardFile.GetLine();

			Dictionary<string, Variant> leaderBoardData;

			try
			{
				var parseResult = Json.ParseString(boardString);
				if (parseResult.VariantType == Variant.Type.Nil)
				{
					throw new Exception();
				}
				leaderBoardData = (Dictionary<string, Variant>)parseResult;
			}
			catch (Exception)
			{
				// Arquivo inválido.
				return;
			}

			Array<Dictionary<string, Variant>> players = (Array<Dictionary<string, Variant>>)leaderBoardData["placar"];

			foreach (Dictionary<string, Variant> player in players)
			{
				LeaderboardItem item = LeaderboardItemScene.Instantiate<LeaderboardItem>();

				item.PlayerName = (string)player["jogador"];
				item.Floor = (string)player["andar_mais_fundo"];
				item.Kills = (string)player["inimigos_mortos"];
				item.Damage = (string)player["dano_tomado"];

				leaderboard.AddChild(item);
			}

		}
	}
}
