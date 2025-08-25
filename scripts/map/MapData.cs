using Godot;
using System;
using System.Runtime.InteropServices;

public partial class MapData : RefCounted
{
	public static readonly TileDefinition wallDefinition = GD.Load<TileDefinition>("res://assets/definitions/tiles/wall.tres");
	public static readonly TileDefinition floorDefinition = GD.Load<TileDefinition>("res://assets/definitions/tiles/floor.tres");

	public int Width { get; private set; }
	public int Height { get; private set; }

	public Godot.Collections.Array<Tile> Tiles { get; private set; } = [];

	public Godot.Collections.Array<Actor> Actors { get; private set; } = [];

	public MapData(int width, int height) {
		Width = width;
		Height = height;

		SetupTiles();
	}

	private void SetupTiles() {
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				Tiles.Add(new Tile(new Vector2I(j, i), wallDefinition));
			}
		}
	}

	public void InsertActor(Actor actor) {
		Actors.Add(actor);
	}

	private int GridToIndex(Vector2I pos) {
		if (!IsInBounds(pos)) return -1;

		return pos.Y * Width + pos.X;
	}

	private bool IsInBounds(Vector2I pos) {
		if (pos.X < 0 || pos.Y < 0) {
			return false;
		}
		if (pos.X >= Width || pos.Y >= Height) {
			return false;
		}

		return true;
	}

	public Tile GetTile(Vector2I pos) {
		int index = GridToIndex(pos);

		if (index < 0) return null;

		return Tiles[index];
	}

	public Tile GetTile(int x, int y) {
		return GetTile(new Vector2I(x, y));
	}

	public Actor GetBlockingActorAtPosition(Vector2I pos) {
		foreach (Actor actor in Actors) {
			if (actor.GridPosition == pos && actor.BlocksMovement) {
				return actor;
			}
		}
		return null;
	}

	public bool IsTileWalkable(Vector2I pos) {
		Tile tile = GetTile(pos);

		if (tile == null) return false;

		return tile.IsWalkable;
	}
}
