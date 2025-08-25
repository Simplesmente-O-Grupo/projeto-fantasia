using Godot;
using System;

public partial class Map : Node2D
{
	public MapData Map_Data { get; private set; }

	[Export]
	private int fovRadius = 12;

	private DungeonGenerator generator;

	FieldOfView fieldOfView;

	private Node2D tilesNode;
	private Node2D actorsNode;

	public override void _Ready()
	{
		base._Ready();

		generator = GetNode<DungeonGenerator>("Generator");
		fieldOfView = GetNode<FieldOfView>("FieldOfView");
		tilesNode = GetNode<Node2D>("Tiles");
		actorsNode = GetNode<Node2D>("Actors");
	}

	private void PlaceTiles() {
		foreach (Tile tile in Map_Data.Tiles) {
			tilesNode.AddChild(tile);
		}
	}

	private void PlaceActors() {
		foreach (Actor actor in Map_Data.Actors) {
			actorsNode.AddChild(actor);
		}
	}

	public void Generate(Player player)
	{
		Map_Data = generator.GenerateDungeon(player);

		player.Map_Data = Map_Data;

		PlaceTiles();
		PlaceActors();
	}

	public void UpdateFOV(Vector2I pos) {
		fieldOfView.UpdateFOV(Map_Data, pos, fovRadius);
	}
}
