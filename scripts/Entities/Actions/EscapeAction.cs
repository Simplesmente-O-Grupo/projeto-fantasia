using System;
using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Actions;

public partial class EscapeAction(Actor actor, bool should_save = false) : Action(actor)
{
	private bool should_save = should_save;
	public override bool Perform()
	{
		if (should_save) {
			Actor.MapData.SaveGame();
		} else {
			// game over
			bool hasLeaderboardFile = FileAccess.FileExists("user://placar.json");
			if (hasLeaderboardFile) {
				using var leaderboardFile = FileAccess.Open("user://placar.json", FileAccess.ModeFlags.ReadWrite);
				string boardString = leaderboardFile.GetLine();

				Dictionary<string, Variant> leaderBoardData;

				try {
					var parseResult = Json.ParseString(boardString);
					if (parseResult.VariantType == Variant.Type.Nil) {
						throw new Exception();
					}
					leaderBoardData = (Dictionary<string, Variant>)parseResult;
				} catch (Exception)
				{
					leaderboardFile.Resize(0);
					leaderboardFile.Seek(0);

					leaderBoardData = new()
					{
						{"placar", new Array<Dictionary<string, Variant>>() {Stats.Instance.Serialize()}}
					};
					boardString = Json.Stringify(leaderBoardData);
				
					leaderboardFile.StoreLine(boardString);

					SignalBus.Instance.EmitSignal(SignalBus.SignalName.EscapeRequested);
					return false;
				}

				Array<Dictionary<string, Variant>> players = (Array<Dictionary<string, Variant>>)leaderBoardData["placar"];

				players.Add(Stats.Instance.Serialize());

				for (int i = 0; i < players.Count; i++) {
					for (int j = 0; j < players.Count - 1 - i; j++) {
						if ((int)players[j]["andar_mais_fundo"] < (int)players[j + 1]["andar_mais_fundo"]) {
							Dictionary<string, Variant> tmp = players[j];
							players[j] = players[j + 1];
							players[j + 1] = tmp;
						}
					}
				}

				if (players.Count > 10) {
					players = players.GetSliceRange(0, 10);
				}

				leaderBoardData["placar"] = players;

				leaderboardFile.Resize(0);
				leaderboardFile.Seek(0);
				leaderboardFile.StoreLine(Json.Stringify(leaderBoardData));
			} else {
				using var leaderboardFile = FileAccess.Open("user://placar.json", FileAccess.ModeFlags.Write);
				Dictionary<string, Variant> leaderBoardData = new()
				{
					{"placar", new Array<Dictionary<string, Variant>>() {Stats.Instance.Serialize()}}
				};
				string boardString = Json.Stringify(leaderBoardData);
				
				leaderboardFile.StoreLine(boardString);
			}
		}
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.EscapeRequested);
		return false;
	}
}