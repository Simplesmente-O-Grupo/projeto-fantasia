using Godot;
using TheLegendOfGustav.Magic;
using TheLegendOfGustav.Map;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Actors;

/// <summary>
/// A classe de ator define um personagem no jogo.
/// </summary>
[GlobalClass]
public partial class Actor : Entity
{
	#region Fields
	private int mp;
	private int hp;

	private int energy;
	#endregion

	#region Constructors
	public Actor(Vector2I initialPosition, MapData map, ActorDefinition definition) : base(initialPosition, map, definition)
	{
		SetDefinition(definition);
	}
	#endregion

	#region Signals
	/// <summary>
	/// Sinal emitido toda vez que o HP mudar.
	/// </summary>
	/// <param name="hp">Novo HP</param>
	/// <param name="maxHp">Quantidade máxima de HP.</param>
	[Signal]
	public delegate void HealthChangedEventHandler(int hp, int maxHp);

	/// <summary>
    /// Sinal emitido toda vez que a mana mudar.
    /// </summary>
    /// <param name="mp">Nova mana</param>
    /// <param name="maxMp">Quantidade máxima de mana</param>
	[Signal]
	public delegate void ManaChangedEventHandler(int mp, int maxMp);

	/// <summary>
	/// Sinal emitido se o ator morrer.
	/// </summary>
	[Signal]
	public delegate void DiedEventHandler();
	#endregion

	#region Properties
	/// <summary>
	/// Se o ator está vivo.
	/// </summary>
	public bool IsAlive { get => Hp > 0; }

	/// <summary>
	/// Utilizado no sistema de turnos.
	/// Enquanto o ator tiver energia, ele poderá realizar turnos.
	/// </summary>
	public int Energy
	{
		get => energy;
		set
		{
			if (value > Speed)
			{
				energy = Speed;
			}
			else
			{
				energy = value;
			}
		}
	}
	/// <summary>
	/// Taxa de recarga de energia.
	/// </summary>
	public int Speed { get => Definition.Speed; }

	/// <summary>
	/// HP máximo do ator.
	/// </summary>
	public int MaxHp { get; private set; }
	/// <summary>
	/// HP atual do ator.
	/// </summary>
	public int Hp
	{
		get => hp;
		set
		{
			// Esta propriedade impede que o HP seja maior que o máximo.
			hp = int.Clamp(value, 0, MaxHp);
			EmitSignal(SignalName.HealthChanged, Hp, MaxHp);
			if (hp <= 0)
			{
				Die();
			}
		}
	}

	/// <summary>
	/// Máximo de mana do ator.
	/// </summary>
	public int MaxMp { get; private set; }
	/// <summary>
	/// Mana atual do ator.
	/// </summary>
	public int Mp
	{
		get => mp;
		set
		{
			mp = int.Clamp(value, 0, MaxMp);
			EmitSignal(SignalName.ManaChanged, Mp, MaxMp);
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

	/// <summary>
    /// Quantos turnos para recarregar a mana.
    /// </summary>
	public int MpRegenRate { get; private set; } = 2;
	/// <summary>
    /// Quanto de mana para recarregar.
    /// </summary>
	public int MpRegenPerTurn { get; private set; } = 5;

	/// <summary>
	/// A definição do ator possui caracterísitcas padrões que definem
	/// o ator em questão.
	/// </summary>
	private ActorDefinition Definition
	{
		get;
		set;
	}

	public SpellBook SpellBook { get; private set; } = new();
	#endregion

	#region Methods
	/// <summary>
	/// Recarrega a energia do ator.
	/// </summary>
	private void RechargeEnergy()
	{
		Energy += Speed;
	}

	/// <summary>
    /// Executado uma vez por 
    /// </summary>
    /// <param name="turn">Número do turno.</param>
	public void OnTurnStart(int turn)
	{
		RechargeEnergy();

		if (turn % MpRegenRate == 0 && Mp < MaxMp)
		{
			Mp += MpRegenPerTurn;
		}
	}

	/// <summary>
	/// Move o ator para uma localização. Veja MovementAction.
	/// </summary>
	/// <param name="offset">Vetor que parte da posição do ator até o seu destino.</param>
	public void Walk(Vector2I offset)
	{
		// Cada ator tem um peso no sistema de pathfinding.
		// Sempre que ele se mover, removemos seu peso da posição antiga
		MapData.UnregisterBlockingEntity(this);
		GridPosition += offset;
		// E colocamos na próxima.
		MapData.RegisterBlockingEntity(this);
		// Este peso influencia o algoritmo de pathfinding.
		// Atores evitam caminhos bloqueados. por outros atores.
	}


	/// <summary>
	/// Recupera uma quantidade de HP do ator.
	/// </summary>
	/// <param name="amount">HP para recuperar</param>
	/// <returns>Quanto HP foi realmente recuperado.</returns>
	public int Heal(int amount)
	{
		int neoHp = Hp + amount;

		if (neoHp > MaxHp) neoHp = MaxHp;

		int recovered = neoHp - Hp;
		Hp = neoHp;
		return recovered;
	}

	/// <summary>
	/// Aplica uma definição de NPC para o ator.
	/// Se o ator for um boneco de barro, este método é como um
	/// sopro de vida.
	/// </summary>
	/// <param name="definition">A definição do ator.</param>
	public virtual void SetDefinition(ActorDefinition definition)
	{
		base.SetDefinition(definition);
		Definition = definition;

		Type = definition.Type;

		MaxHp = definition.Hp;
		Hp = definition.Hp;
		MaxMp = definition.Mp;
		Mp = definition.Mp;

		Atk = definition.Atk;
		Def = definition.Def;
		Men = definition.Men;
	}

	public virtual void Die()
	{
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

		if (MapData.Player == this)
		{
			deathMessage = "Você morreu!";
		}
		else
		{
			deathMessage = $"{DisplayName} morreu!";
		}

		MessageLogData.Instance.AddMessage(deathMessage);

		Texture = Definition.deathTexture;
		BlocksMovement = false;
		Type = EntityType.CORPSE;
		DisplayName = $"Restos mortais de {DisplayName}";
		MapData.UnregisterBlockingEntity(this);
		EmitSignal(SignalName.Died);
	}
	#endregion
}