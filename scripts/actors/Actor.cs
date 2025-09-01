using Godot;

/// <summary>
/// A classe de ator define um personagem no jogo.
/// </summary>
[GlobalClass]
public abstract partial class Actor : Sprite2D
{
	/// <summary>
    /// Sinal emitido toda vez que o HP mudar.
    /// </summary>
    /// <param name="hp">Novo HP</param>
    /// <param name="maxHp">Quantidade máxima de HP.</param>
	[Signal]
	public delegate void HealthChangedEventHandler(int hp, int maxHp);

	/// <summary>
    /// Sinal emitido se o ator morrer.
    /// </summary>
	[Signal]
	public delegate void DiedEventHandler();


	/// <summary>
	/// A definição do ator possui caracterísitcas padrões que definem
	/// o ator em questão.
	/// </summary>
	protected ActorDefinition definition;
	/// <summary>
    /// É conveniente ter acesso ao mapa dentro do ator. Isto porque suas ações são feitas dentro
    /// do mapa, então é necessário ter acesso à algumas informações.
    /// </summary>
	public MapData Map_Data { get; set; }

	/// <summary>
    /// Se o ator está vivo.
    /// </summary>
	public bool IsAlive { get => Hp > 0; }

	private int energy;
	/// <summary>
	/// Utilizado no sistema de turnos.
	/// Enquanto o ator tiver energia, ele poderá realizar turnos.
	/// </summary>
	public int Energy 
	{ 
		get => energy;
		set
		{
			if (value > Speed) {
				energy = Speed;
			} else {
				energy = value;
			}
		}
	}
	/// <summary>
    /// Taxa de recarga de energia.
    /// </summary>
    public int Speed { get => definition.Speed; }

	/// <summary>
    /// Executado uma vez por turno,
    /// </summary>
	public void RechargeEnergy() {
		Energy += Speed;
	}

	private Vector2I gridPosition = Vector2I.Zero;
	/// <summary>
    /// Posição do ator no mapa do jogo. Diferentemente de Position, GridPosition tem como formato 
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
	/// Se o ator bloqueia movimento (não pode oculpar a mesma célula de outro ator.)
	/// </summary>
	public bool BlocksMovement {
		get => blocksMovement;
	}

	private string actorName;
	/// <summary>
    /// Nome do ator.
    /// </summary>
	public string ActorName {
		get => actorName;
	}

	private int hp;
	/// <summary>
    /// HP máximo do ator.
    /// </summary>
	public int MaxHp { get; private set; }
	/// <summary>
    /// HP atual do ator.
    /// </summary>
	public int Hp {
		get => hp;
		set {
			// Esta propriedade impede que o HP seja maior que o máximo.
			hp = int.Clamp(value, 0, MaxHp);
			EmitSignal(SignalName.HealthChanged, Hp, MaxHp);
			if (hp <= 0) {
				Die();
			}
		}
	}

	private int mp;
	/// <summary>
    /// Máximo de mana do ator.
    /// </summary>
	public int MaxMp { get; private set; }
	/// <summary>
    /// Mana atual do ator.
    /// </summary>
	public int Mp {
		get => mp;
		set {
			mp = int.Clamp(value, 0, MaxMp);
		}
	}

	/// <summary>
    /// Estatística de ataque
    /// </summary>
	public int Atk { get; private set; }

	/// <summary>
    /// Estatística de defesa.
    /// </summary>
	public int Def { get; private set; }

	/// <summary>
    /// Estatística mental.
    /// </summary>
	public int Men { get; private set; }

	public override void _Ready()
	{
		base._Ready();
		// Quando o ator for carregado completamente, atualizamos sua posição para refletir
		// sua posição real.
		GridPosition = Grid.WorldToGrid(Position);
	}

	/// <summary>
    /// Move o ator para uma localização. Veja MovementAction.
    /// </summary>
    /// <param name="offset">Vetor que parte da posição do ator até o seu destino.</param>
	public void Walk(Vector2I offset) {
		// Cada ator tem um peso no sistema de pathfinding.
		// Sempre que ele se mover, removemos seu peso da posição antiga
		Map_Data.UnregisterBlockingActor(this);
		GridPosition += offset;
		// E colocamos na próxima.
		Map_Data.RegisterBlockingActor(this);
		// Este peso influencia o algoritmo de pathfinding.
		// Atores evitam caminhos bloqueados. por outros atores.
	}

	public Actor(Vector2I initialPosition, MapData map, ActorDefinition definition) {
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
	public virtual void SetDefinition(ActorDefinition definition) {
		this.definition = definition;
		blocksMovement = definition.blocksMovement;
		actorName = definition.name;
		ZIndex = 1;
		Texture = definition.texture;

		MaxHp = definition.Hp;
		Hp = definition.Hp;
		MaxMp = definition.Mp;
		Mp = definition.Mp;

		Atk = definition.Atk;
		Def = definition.Def;
		Men = definition.Men;
	}

	public virtual void Die() {
		//⠀⠀⠀⠀⢠⣤⣤⣤⢠⣤⣤⣤⣤⣄⢀⣠⣤⣤⣄⠀⠀⠀⢀⣠⣤⣤⣄⠀⣤⣤⠀⠀⣠⣤⣤⣤⣤⣤⡄⢠⣤⣤⣤⣄⠀⠀
		//⠀⠀⠀⠀⠈⢹⣿⠉⠈⠉⣿⣿⠉⠉⢾⣿⣉⣉⠙⠀⠀⢀⣾⡟⠉⠉⣿⣧⢸⣿⡄⢠⣿⠏⣿⣿⣉⣉⡁⢸⣿⡏⢉⣿⡷⠀
		//⠀⠀⠀⠀⠀⢸⣿⠀⠀⠀⣿⣿⠀⠀⠈⠿⠿⣿⣿⡀⠀⠸⣿⡇⠀⠀⣾⣿⠀⢿⣿⣸⡿⠀⣿⣿⠿⠿⠇⢸⣿⣿⣿⣿⠀⠀
		//⠀⠀⠀⠀⢠⣼⣿⣤⠀⠀⣿⣿⠀⠀⢷⣦⣤⣼⡿⠁⠀⠀⠹⣿⣤⣴⡿⠋⠀⠘⣿⣿⠃⠀⣿⣿⣤⣤⡄⢸⣿⡇⠙⢿⣦⡀
		//⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⠀⠀⠀⠀⠀⠀⢀⣰⣶⣶⣶⣿⣿⣿⣿⣷⣶⣤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⠀⠀⠀⠀⢀⣾⣿⣿⣿⣿⣿⠿⠛⠛⠻⢿⣿⣿⣿⣿⣿⣿⣿⣶⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⠀⠀⠀⢀⢾⣿⣿⣿⣿⠟⠁⠀⠀⠀⠀⠀⠈⠉⠉⠉⠻⢿⢿⣿⣿⣦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⠀⠀⢠⠏⢸⣿⣿⡿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠛⠿⢻⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⠀⢀⠇⠀⠈⠿⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⢀⡞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠰⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⡸⠀⠀⠀⠀⠀⠀⠀⠀⡼⠛⠳⣄⡀⠀⠐⢿⣦⡀⠀⠀⠀⢠⠃⠀⣸⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⢠⠇⠀⠀⠀⠀⠀⠀⠀⠀⠉⠀⠀⠀⠉⣳⠟⠒⠻⣿⣦⡀⠀⡘⠀⢰⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⢀⠞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⠃⢠⣄⡀⠈⠙⢿⡌⠁⠀⡞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠳⣄⣈⢻⡿⠃⢰⠟⠲⣼⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⠀⡰⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠛⡶⢴⠋⠀⠀⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⡴⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠞⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⡴⢟⠒⠀⠀⠀⠀⢰⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⠏⠀⠀⠈⠉⣿⠇⠀⢀⡎⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⣷⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠠⣤⣤⣀⢰⠏⠉⠙⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⣿⣿⣆⠀⠀⠀⠀⠀⠀⠀⣀⣀⣀⣀⣠⠴⠢⠦⠽⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⣿⣿⣿⣷⡄⣀⡀⠈⠉⠋⢹⠋⠁⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//⠿⠿⠿⠿⠿⠦⠈⠀⠀⠀⠸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀

		string deathMessage;

		if (Map_Data.Player == this) {
			deathMessage = "Você morreu!";
		} else {
			deathMessage = $"{ActorName} morreu!";
		}

		MessageLog.SendMessage(deathMessage);

		Texture = definition.deathTexture;
		blocksMovement = false;
		ZIndex = 0;
		actorName = $"Restos mortais de {actorName}";
		Map_Data.UnregisterBlockingActor(this);
		EmitSignal(SignalName.Died);
	}
}