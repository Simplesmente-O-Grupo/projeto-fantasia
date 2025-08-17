using Godot;
using System;

public abstract partial class Grid : GodotObject {
	public static readonly Vector2I tileSize = new(16, 16);

	public static Vector2I WorldToGrid(Vector2 coord) {
		return (Vector2I)(coord / tileSize);
	}

	public static Vector2 GridToWorld(Vector2I coord) {
		return coord * tileSize;
	}
}
