using Godot;

public abstract partial class Actor : Node2D {
	static int baseWalkCost = 10;
	[Export]
	public TileMapLayer Map { get; set; }
	[Signal]
	public delegate void actionPerformedEventHandler();

	[Export]
	public int Energy { get; set; } = 0;
	[Export]
	public int Speed { get; protected set; } = 10;

	protected void Walk(Vector2I offset) {
		Vector2I toMovePos = Map.LocalToMap(Position);
		toMovePos += offset;
		
		TileData tile = Map.GetCellTileData(toMovePos);

		if (tile.HasCustomData("isWalkable") && (bool) tile.GetCustomData("isWalkable")) {
			GD.Print(toMovePos);
			Position = Map.MapToLocal(toMovePos);
		}

		Energy -= baseWalkCost;
		EndAction();
	}

	protected virtual void EndAction() {
		EmitSignal(SignalName.actionPerformed);
	}

	public abstract void performAction();
}