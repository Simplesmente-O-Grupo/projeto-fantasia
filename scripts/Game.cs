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
	/// <summary>
	/// Definição de um jogador.
	/// </summary>
	private static readonly PlayerDefinition playerDefinition = GD.Load<PlayerDefinition>("res://assets/definitions/actor/Player.tres");
	/// <summary>
	/// O jogo possui o mapa.
	/// </summary>
	private Map.Map Map { get; set; }
	/// <summary>
	/// Objeto para obter input do usuário.
	/// </summary>
	private InputHandler InputHandler { get; set; }
	/// <summary>
	/// Gerenciador de turnos
	/// </summary>
	private TurnManager TurnManager { get; set; }

	private Hud hud;

	[Signal]
	public delegate void MainMenuRequestedEventHandler();

	private SignalBus.EscapeRequestedEventHandler escapeLambda;

	public override void _Ready()
	{
		base._Ready();

		escapeLambda = () => EmitSignal(SignalName.MainMenuRequested);
		SignalBus.Instance.EscapeRequested += escapeLambda;

		Map = GetNode<Map.Map>("Map");

		InputHandler = GetNode<InputHandler>("InputHandler");
		hud = GetNode<Hud>("HUD");

		// O jogador é criado pelo jogo.
		Player player = new Player(Vector2I.Zero, null, playerDefinition);
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		RemoveChild(camera);
		player.HealthChanged += (int hp, int maxHp) => hud.OnHealthChanged(hp, maxHp);
		player.ManaChanged += hud.OnManaChanged;
		player.Died += () => InputHandler.SetInputHandler(InputHandlers.GameOver);

		player.AddChild(camera);

		Map.Generate(player);

		Map.UpdateFOV(player.GridPosition);

		TurnManager = new(Map);

		MessageLogData.Instance.AddMessage("Boa sorte!");
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

		Player player = Map.MapData.Player;

		// Pegamos uma ação do usuário
		Action action = InputHandler.GetAction(player);

		if (action != null)
		{
			TurnManager.InsertPlayerAction(action);
		}

		// Computamos um turno.
		TurnManager.Tick();
	}

}
