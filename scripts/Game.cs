using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.InputHandling;
using TheLegendOfGustav.GUI;
using TheLegendOfGustav.Time;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav;

/// <summary>
/// Classe principal do jogo.
/// Lar da lógica central do jogo.
/// </summary>
public partial class Game : Node
{
	private bool initialized = false;
	/// <summary>
	/// Definição de um jogador.
	/// </summary>
	private static readonly PlayerDefinition playerDefinition = GD.Load<PlayerDefinition>("res://assets/definitions/actor/Player.tres");
	/// <summary>
	/// O jogo possui o mapa.
	/// </summary>
	private Map.Map map;
	/// <summary>
	/// Objeto para obter input do usuário.
	/// </summary>
	private InputHandler inputHandler;
	/// <summary>
	/// Gerenciador de turnos
	/// </summary>
	private TurnManager turnManager;

	private Hud hud;

	private SignalBus.EscapeRequestedEventHandler escapeLambda;

	[Signal]
	public delegate void MainMenuRequestedEventHandler();

	public override void _Ready()
	{
		base._Ready();
		escapeLambda = () => EmitSignal(SignalName.MainMenuRequested);
		SignalBus.Instance.EscapeRequested += escapeLambda;

	}
	public void NewGame(string name)
	{
		map = GetNode<Map.Map>("Map");

		inputHandler = GetNode<InputHandler>("InputHandler");
		hud = GetNode<Hud>("HUD");

		// O jogador é criado pelo jogo.
		Player player = new Player(Vector2I.Zero, null, playerDefinition);
		if (name != "")
		{
			player.DisplayName = name;
		}
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		RemoveChild(camera);
		player.HealthChanged += (int hp, int maxHp) => hud.OnHealthChanged(hp, maxHp);
		player.ManaChanged += hud.OnManaChanged;
		player.Died += () => inputHandler.SetInputHandler(InputHandlers.GameOver);

		player.AddChild(camera);

		map.Generate(player);

		map.UpdateFOV(player.GridPosition);

		turnManager = new(map);

		MessageLogData.Instance.AddMessage("Boa sorte!");
		initialized = true;
	}
	public bool LoadGame()
	{
		map = GetNode<Map.Map>("Map");

		inputHandler = GetNode<InputHandler>("InputHandler");
		hud = GetNode<Hud>("HUD");

		// O jogador é criado pelo jogo.
		Player player = new Player(Vector2I.Zero, null, playerDefinition);
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		RemoveChild(camera);
		
		player.AddChild(camera);

		Stats.Instance.PlayerName = player.DisplayName;

		if (!map.LoadGame(player))
		{
			return false;
		}
		
		player.HealthChanged += (int hp, int maxHp) => hud.OnHealthChanged(hp, maxHp);
		player.ManaChanged += hud.OnManaChanged;
		player.Died += () => inputHandler.SetInputHandler(InputHandlers.GameOver);

		map.UpdateFOV(player.GridPosition);

		turnManager = new(map);

		MessageLogData.Instance.AddMessage("Boa sorte!");
		initialized = true;
		return true;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
		{
			if (escapeLambda != null)
			{
				SignalBus.Instance.EscapeRequested -= escapeLambda;
			}
		}
		base._Notification(what);
	}


	/// <summary>
	/// Método executa aproximadamente 60 vezes por segundo.
	/// </summary>
	/// <param name="delta"></param>
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if(initialized)
		{
		Player player = map.MapData.Player;

		// Pegamos uma ação do usuário
		Action action = inputHandler.GetAction(player);

		if (action != null)
		{
			turnManager.InsertPlayerAction(action);
		}

		// Computamos um turno.
		turnManager.Tick();

		}
	}

}
