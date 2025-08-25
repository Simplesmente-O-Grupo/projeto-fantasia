using Godot;

[GlobalClass]
public abstract partial class Actor : Sprite2D
{
	private ActorDefinition definition;
	private Vector2I gridPosition = Vector2I.Zero;
	public MapData Map_Data { get; set; }

	public Actor(Vector2I initialPosition, MapData map, ActorDefinition definition) {
		GridPosition = initialPosition;
		Map_Data = map;
		this.definition = definition;
		Texture = definition.texture;
		Centered = false;
	}

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