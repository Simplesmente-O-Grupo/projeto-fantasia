using Godot;
using System;

public partial class Game : Node {
	private static readonly ActorDefinition playerDefinition = GD.Load<ActorDefinition>("res://assets/definitions/actor/Player.tres");
	private static readonly ActorDefinition skeletonDefinition = GD.Load<ActorDefinition>("res://assets/definitions/actor/Skeleton.tres");
	private Player player;
	private Node2D actorsNode;
	public MapData Map_Data { get; private set; }
	private InputHandler inputHandler;

	public override void _Ready() {
		base._Ready();

		Map Map = GetNode<Map>("Map");

		inputHandler = GetNode<InputHandler>("InputHandler");
		actorsNode = GetNode<Node2D>("Actors");

		player = new Player(Vector2I.Zero, null, playerDefinition);
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		RemoveChild(camera);

		player.AddChild(camera);

		actorsNode.AddChild(player);

		Map.Generate(player);

		Map_Data = Map.Map_Data;
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
		foreach (Actor actor in Map_Data.Actors) {
			if (actor is Player) continue;

			GD.Print($"O {actor.ActorName} foi cuckado e não tem como agir.");
		}
	}
}
