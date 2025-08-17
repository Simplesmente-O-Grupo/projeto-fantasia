using Godot;

[GlobalClass]
public abstract partial class Actor : Sprite2D {
	private Vector2I gridPosition = Vector2I.Zero;
	public Vector2I GridPosition {
		set {
			gridPosition = value;
			Position = Grid.GridToWorld(value);
		}
		get => gridPosition;
	}

	public override void _Ready()
	{
		base._Ready();
		GridPosition = Grid.WorldToGrid(Position);
	}

	public void Walk(Vector2I offset) {
		GridPosition += offset;
	}
}