using Godot;
using System;

public partial class DungeonLevel : Node2D
{
	public Player player;
	public TileMapLayer buildingLayer;

	private Node2D actors;

	public override void _Ready()
	{
		base._Ready();

		buildingLayer = GetNode<TileMapLayer>("Dungeon");
		actors = GetNode<Node2D>("Actors");
		player = actors.GetNode<Player>("Player");
	}
}
