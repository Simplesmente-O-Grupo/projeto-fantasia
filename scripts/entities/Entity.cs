using Godot;

/// <summary>
/// Defino aqui que o jogo irá desenhar
/// atores em cima de itens e itens acima de corpos.
/// </summary>
public enum EntityType
{
	CORPSE,
	ITEM,
	ACTOR
};

/// <summary>
/// Classe para elementos móveis que o jogador pode interagir.
/// </summary>
public abstract partial class Entity : Sprite2D {
	/// <summary>
	/// A definição da entidade possui caracterísitcas padrões que definem
	/// a entidade em questão.
	/// </summary>
	private EntityDefinition definition;

	private EntityType type;
	/// <summary>
    /// Usado para definir a camada da entidade no mapa.
    /// </summary>
	public EntityType Type {
		get => type;
		set {
			type = value;
			ZIndex = (int) type;
		}
	}

	/// <summary>
	/// É conveniente ter acesso ao mapa dentro da entidade. Isto porque ela existe dentro
	/// do mapa, então é necessário ter acesso à algumas informações.
	/// </summary>
	public MapData Map_Data { get; set; }

	private Vector2I gridPosition = Vector2I.Zero;
	/// <summary>
    /// Posição da entidade no mapa do jogo. Diferentemente de Position, GridPosition tem como formato 
    /// os tiles do mapa.
    /// </summary>
	public Vector2I GridPosition {
		set {
			gridPosition = value;
			// O sistema de coordenadas do Godot é em pixels, mas faz mais sentido para o jogo utilizar coordenadas em tiles.
			// Esta propriedade converte um sistema para o outro automaticamente.
			Position = Grid.GridToWorld(value);
		}
		get => gridPosition;
	}

	private bool blocksMovement;
	/// <summary>
	/// Se a entidade bloqueia movimento (não pode oculpar a mesma célula de outra entidade.)
	/// </summary>
	public bool BlocksMovement {
		get => blocksMovement;
		protected set {
			blocksMovement = value;
		}
	}

	private string displayName;
	/// <summary>
    /// Nome da entidade.
    /// </summary>
	public string DisplayName {
		get => displayName;
		protected set {
			displayName = value;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		// Quando a entidade for carregada completamente, atualizamos sua posição para refletir
		// sua posição real.
		GridPosition = Grid.WorldToGrid(Position);
	}

	public Entity(Vector2I initialPosition, MapData map, EntityDefinition definition) {
		GridPosition = initialPosition;
		Map_Data = map;
		Centered = false;

		SetDefinition(definition);
	}

	/// <summary>
    /// Aplica uma definição de NPC para o ator.
    /// Se o ator for um boneco de barro, este método é como um
    /// sopro de vida.
    /// </summary>
    /// <param name="definition">A definição do ator.</param>
	public virtual void SetDefinition(EntityDefinition definition) {
		this.definition = definition;
		BlocksMovement = definition.blocksMovement;
		DisplayName = definition.name;
		Type = definition.Type;
		Texture = definition.texture;
	}
}