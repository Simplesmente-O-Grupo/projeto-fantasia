using Godot;
using System;

public partial class Map : Node2D
{
	public MapData Map_Data { get; private set; }

	[Export]
	public int Height;
	[Export]
	public int Width;

	private void PlaceTiles() {
		foreach (Tile tile in Map_Data.Tiles) {
			AddChild(tile);
		}
	}

	public override void _Ready()
	{
		base._Ready();

		Map_Data = new MapData(Width, Height);

		PlaceTiles();
	}
}
