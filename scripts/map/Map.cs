using Godot;
using System;

public partial class Map : Node2D
{
	public MapData Map_Data { get; private set; }

	DungeonGenerator generator;

	private void PlaceTiles() {
		foreach (Tile tile in Map_Data.Tiles) {
			AddChild(tile);
		}
	}

	public void Generate(Player player)
	{
		generator = GetNode<DungeonGenerator>("Generator");

		Map_Data = generator.GenerateDungeon(player);

		Map_Data.InsertActor(player);

		player.Map_Data = Map_Data;

		PlaceTiles();
	}
}
