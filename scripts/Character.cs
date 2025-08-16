using Godot;
using System;

public partial class Character : Sprite2D {
	[Export]
	public TileMapLayer map;
	public override void _Process(double delta) {
		base._Process(delta);

		Vector2I offset = Vector2I.Zero;

		if (Input.IsActionJustPressed("walk-up")) {
			offset += Vector2I.Up;
		}
		if (Input.IsActionJustPressed("walk-down")) {
			offset += Vector2I.Down;
		}
		if (Input.IsActionJustPressed("walk-left")) {
			offset += Vector2I.Left;
		}
		if (Input.IsActionJustPressed("walk-right")) {
			offset += Vector2I.Right;
		}

		if (offset != Vector2I.Zero) {
			Walk(offset);
		}
	}

	private void Walk(Vector2I offset) {
		Vector2I gridCoords = map.LocalToMap(Position);
		gridCoords += offset;

		TileData tile = map.GetCellTileData(gridCoords);

		
		if (tile.HasCustomData("isWalkable") && (bool) tile.GetCustomData("isWalkable")) {
			GD.Print(gridCoords);
			Position = map.MapToLocal(gridCoords);
		}
	}
}
