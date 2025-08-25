using Godot;
using System;

public partial class Game : Node {
	private static readonly ActorDefinition playerDefinition = GD.Load<ActorDefinition>("res://assets/definitions/actor/Player.tres");
	private static readonly ActorDefinition skeletonDefinition = GD.Load<ActorDefinition>("res://assets/definitions/actor/Skeleton.tres");
	private Player player;
	private Node2D actorsNode;
	private Map Map;
	private InputHandler inputHandler;

	public override void _Ready() {
		base._Ready();

		Map = GetNode<Map>("Map");

		inputHandler = GetNode<InputHandler>("InputHandler");
		actorsNode = GetNode<Node2D>("Actors");

		player = new Player(Vector2I.Zero, null, playerDefinition);
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		RemoveChild(camera);

		player.AddChild(camera);

		actorsNode.AddChild(player);

		Map.Generate(player);

		Map.UpdateFOV(player.GridPosition);
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		Action action = inputHandler.GetAction(player);

		if (action != null) {
			Vector2I previousPlayerPos = player.GridPosition;
			action.Perform();
			HandleEnemyTurns();
			if (player.GridPosition != previousPlayerPos) {
				Map.UpdateFOV(player.GridPosition);
			}
		}
	}

	private void HandleEnemyTurns() {
		foreach (Actor actor in Map.Map_Data.Actors) {
			if (actor is Player) continue;

			GD.Print($"O {actor.ActorName} foi cuckado e não tem como agir.");
		}
	}
}
