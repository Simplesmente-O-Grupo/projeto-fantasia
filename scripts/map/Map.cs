using Godot;
using System;

public partial class Map : Node2D
{
	public MapData Map_Data { get; private set; }

	[Export]
	private int fovRadius = 12;

	DungeonGenerator generator;

	FieldOfView fieldOfView;

	public override void _Ready()
	{
		base._Ready();

		generator = GetNode<DungeonGenerator>("Generator");
		fieldOfView = GetNode<FieldOfView>("FieldOfView");
	}

	private void PlaceTiles() {
		foreach (Tile tile in Map_Data.Tiles) {
			AddChild(tile);
		}
	}

	public void Generate(Player player)
	{
		Map_Data = generator.GenerateDungeon(player);

		Map_Data.InsertActor(player);

		player.Map_Data = Map_Data;

		PlaceTiles();
	}

	public void UpdateFOV(Vector2I pos) {
		fieldOfView.UpdateFOV(Map_Data, pos, fovRadius);
	}
}
