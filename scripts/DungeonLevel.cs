using Godot;
using System;

public partial class DungeonLevel : Node2D
{
	public Player player;
	private TileMapLayer buildingLayer;
	[Export]
	private Godot.Collections.Array<Actor> actors = [];

	private Node2D actorsNode;

	public override void _Ready()
	{
		base._Ready();

		buildingLayer = GetNode<TileMapLayer>("Dungeon");
		actorsNode = GetNode<Node2D>("Actors");
		player = actorsNode.GetNode<Player>("Player");
	}

	public bool IsTileWalkable(Vector2I pos) {
		TileData tile = buildingLayer.GetCellTileData(pos);

		if (tile == null) return false;

		return (bool)tile.GetCustomData("isWalkable");
	}

	public Actor GetBlockingActorAtPosition(Vector2I pos) {
		foreach (Actor actor in actors) {
			if (actor.GridPosition == pos && actor.BlocksMovement) {
				return actor;
			}
		}
		return null;
	}
}
