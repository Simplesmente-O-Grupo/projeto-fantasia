using Godot;
using System;

public partial class Character : Sprite2D {
	[Export]
	public TileMapLayer map;

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (!@event.IsPressed()) return;

		Vector2I offset = Vector2I.Zero;

		if (@event.IsActionPressed("walk-up")) {
			offset += Vector2I.Up;
		}
		if (@event.IsActionPressed("walk-down")) {
			offset += Vector2I.Down;
		}
		if (@event.IsActionPressed("walk-left")) {
			offset += Vector2I.Left;
		}
		if (@event.IsActionPressed("walk-right")) {
			offset += Vector2I.Right;
		}

		if (offset != Vector2I.Zero) {
			Walk(offset);
		}
	}

	private void Walk(Vector2I offset) {
		Vector2I toMovePos = map.LocalToMap(Position);
		toMovePos += offset;
		
		TileData tile = map.GetCellTileData(toMovePos);

		if (tile.HasCustomData("isWalkable") && (bool) tile.GetCustomData("isWalkable")) {
			GD.Print(toMovePos);
			Position = map.MapToLocal(toMovePos);
		}
	}
}
