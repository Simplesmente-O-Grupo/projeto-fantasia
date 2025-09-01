using Godot;


/// <summary>
/// Classe principal do jogo.
/// Lar da lógica central do jogo.
/// </summary>
public partial class Game : Node {
	/// <summary>
    /// Definição de um jogador.
    /// </summary>
	private static readonly ActorDefinition playerDefinition = GD.Load<ActorDefinition>("res://assets/definitions/actor/Player.tres");
	/// <summary>
    /// O jogo possui o mapa.
    /// </summary>
	private Map Map;
	/// <summary>
    /// Objeto para obter input do usuário.
    /// </summary>
	private InputHandler inputHandler;
	/// <summary>
	/// Gerenciador de turnos
	/// </summary>
	private TurnManager turnManager;

	private Hud hud;

	public override void _Ready() {
		base._Ready();

		Map = GetNode<Map>("Map");

		inputHandler = GetNode<InputHandler>("InputHandler");
		hud = GetNode<Hud>("HUD");

		// O jogador é criado pelo jogo.
		Player player = new Player(Vector2I.Zero, null, playerDefinition);
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		RemoveChild(camera);
		player.HealthChanged += (int hp, int maxHp) => hud.OnHealthChanged(hp, maxHp);
		player.Died += () => inputHandler.SetInputHandler(InputHandlers.GameOver);

		player.AddChild(camera);

		Map.Generate(player);

		Map.UpdateFOV(player.GridPosition);

		turnManager = new(Map);

		MessageLog.SendMessage("UMA FILA DE HOMENS EJACULANDO NA BOCA DA DALVA");
	}

	/// <summary>
    /// Método executa aproximadamente 60 vezes por segundo.
    /// </summary>
    /// <param name="delta"></param>
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		Player player = Map.Map_Data.Player;

		// Pegamos uma ação do usuário
		Action action = inputHandler.GetAction(player);

		if (action != null) {
			turnManager.InsertPlayerAction(action);
		}

		// Computamos um turno.
		turnManager.Tick();
	}

}
