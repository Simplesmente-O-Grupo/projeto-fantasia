using Godot;
using System;

public partial class DungeonLevel : Node2D {
	TurnManager turnManager;

	public override void _Ready() {
		base._Ready();

		turnManager = GetNode<TurnManager>("TurnManager");

		turnManager.Tick();
	}

	public void OnTurnEnd() {
		turnManager.Tick();
	}
}
