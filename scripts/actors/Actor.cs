using Godot;

[GlobalClass]
public abstract partial class Actor : Sprite2D
{
	protected ActorDefinition definition;
	public MapData Map_Data { get; set; }

	private Vector2I gridPosition = Vector2I.Zero;
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

	private int hp;
	public int MaxHp { get; private set; }
	public int Hp {
		get => hp;
		set {
			hp = int.Clamp(value, 0, MaxHp);
		}
	}

	private int mp;
	public int MaxMp { get; private set; }
	public int Mp {
		get => mp;
		set {
			mp = int.Clamp(value, 0, MaxMp);
		}
	}

	public int Atk { get; private set; }

	public int Def { get; private set; }

	public int Men { get; private set; }

	public override void _Ready()
	{
		base._Ready();
		GridPosition = Grid.WorldToGrid(Position);
	}

	public void Walk(Vector2I offset) {
		Map_Data.UnregisterBlockingActor(this);
		GridPosition += offset;
		Map_Data.RegisterBlockingActor(this);
	}

	public Actor(Vector2I initialPosition, MapData map, ActorDefinition definition) {
		GridPosition = initialPosition;
		Map_Data = map;
		Centered = false;

		SetDefinition(definition);
	}

	public virtual void SetDefinition(ActorDefinition definition) {
		this.definition = definition;
		Texture = definition.texture;

		MaxHp = definition.Hp;
		Hp = definition.Hp;
		MaxMp = definition.Mp;
		Mp = definition.Mp;

		Atk = definition.Atk;
		Def = definition.Def;
		Men = definition.Men;
	}
}