using Godot;

public partial class Actor : Node2D {
	[Export]
	public TileMapLayer Map { get; set; }

	protected void Walk(Vector2I offset) {
		Vector2I toMovePos = Map.LocalToMap(Position);
		toMovePos += offset;
		
		TileData tile = Map.GetCellTileData(toMovePos);

		if (tile.HasCustomData("isWalkable") && (bool) tile.GetCustomData("isWalkable")) {
			GD.Print(toMovePos);
			Position = Map.MapToLocal(toMovePos);
		}
	}
}