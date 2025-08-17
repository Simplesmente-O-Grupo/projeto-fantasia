using Godot;
using System;

public partial class Game : Node {
	private Player player;
	public TileMapLayer Dungeon { get; private set; }
	private DungeonLevel map;
	private InputHandler inputHandler;

	public override void _Ready() {
		base._Ready();

		map = GetNode<DungeonLevel>("Map");

		inputHandler = GetNode<InputHandler>("InputHandler");
		Dungeon = map.buildingLayer;

		player = map.player;
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		Action action = inputHandler.GetAction();

		action?.Perform(this, player);
	}
}
