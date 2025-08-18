using Godot;
using System;

public partial class Game : Node {
	private static readonly ActorDefinition playerDefinition = GD.Load<ActorDefinition>("res://assets/definitions/actor/Player.tres");
	private Player player;
	public DungeonLevel Map { get; private set; }
	private InputHandler inputHandler;

	public override void _Ready() {
		base._Ready();

		Map = GetNode<DungeonLevel>("Map");

		inputHandler = GetNode<InputHandler>("InputHandler");

		player = new Player(new Vector2I(0, 0), Map, playerDefinition);
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		RemoveChild(camera);

		player.AddChild(camera);

		Map.InsertActor(player);
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
