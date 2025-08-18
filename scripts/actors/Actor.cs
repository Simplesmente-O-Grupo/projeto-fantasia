using Godot;

[GlobalClass]
public abstract partial class Actor : Sprite2D
{
	[Export]
	private ActorDefinition definition;
	private Vector2I gridPosition = Vector2I.Zero;
	[Export]
	public DungeonLevel Map { get; private set; }
	public Vector2I GridPosition {
		set {
			gridPosition = value;
			Position = Grid.GridToWorld(value);
		}
		get => gridPosition;
	}

	public bool BlocksMovement {
		get => definition.blocksMovement;
	}

	public string ActorName {
		get => definition.name;
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