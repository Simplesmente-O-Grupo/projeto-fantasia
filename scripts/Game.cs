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

		Map_Data = Map.Map_Data;

		inputHandler = GetNode<InputHandler>("InputHandler");
		actorsNode = GetNode<Node2D>("Actors");

		player = new Player(Vector2I.Zero, Map_Data, playerDefinition);
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		RemoveChild(camera);

		player.AddChild(camera);

		actorsNode.AddChild(player);

		Enemy Skeleton = new Enemy(Vector2I.Zero, Map_Data, skeletonDefinition);
		actorsNode.AddChild(Skeleton);

		Map_Data.InsertActor(new Vector2I(1, 1), player);
		Map_Data.InsertActor(new Vector2I(3, 4), Skeleton);
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
