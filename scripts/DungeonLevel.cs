using Godot;
using System;

public partial class DungeonLevel : Node2D
{
	public Player player;
	private TileMapLayer buildingLayer;

	private Node2D actors;

	public override void _Ready()
	{
		base._Ready();

		buildingLayer = GetNode<TileMapLayer>("Dungeon");
		actors = GetNode<Node2D>("Actors");
		player = actors.GetNode<Player>("Player");
	}

	public bool IsTileWalkable(Vector2I pos) {
		TileData tile = buildingLayer.GetCellTileData(pos);

		if (tile == null) return false;

		return (bool)tile.GetCustomData("isWalkable");
	}
}
