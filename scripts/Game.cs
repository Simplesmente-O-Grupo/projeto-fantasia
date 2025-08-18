using Godot;
using System;

public partial class Game : Node {
	private Player player;
	public DungeonLevel Map { get; private set; }
	private InputHandler inputHandler;

	public override void _Ready() {
		base._Ready();

		Map = GetNode<DungeonLevel>("Map");

		inputHandler = GetNode<InputHandler>("InputHandler");
		
		player = Map.player;
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		Action action = inputHandler.GetAction(player);

		if (action != null) {
			action.Perform();
			HandleEnemyTurns();
		}
	}

	private void HandleEnemyTurns() {
		foreach (Actor actor in Map.Actors) {
			if (actor is Player) continue;

			GD.Print($"O {actor.ActorName} foi cuckado e não tem como agir.");
		}
	}
}
