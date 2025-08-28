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

	public override void _Ready() {
		base._Ready();

		Map = GetNode<Map>("Map");

		inputHandler = GetNode<InputHandler>("InputHandler");

		// O jogador é criado pelo jogo.
		Player player = new Player(Vector2I.Zero, null, playerDefinition);
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		RemoveChild(camera);

		player.AddChild(camera);

		Map.Generate(player);

		Map.UpdateFOV(player.GridPosition);
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

		// Se realmente houve uma ação, computamos um turno.
		if (action != null) {
			Vector2I previousPlayerPos = player.GridPosition;
			// Primeiro executamos a ação do jogador
			action.Perform();
			// Depois computamos os turnos dos outros atores.
			HandleEnemyTurns();
			// Por fim, se o jogador mudou de lugar, atualizamos seu campo de visão.
			if (player.GridPosition != previousPlayerPos) {
				Map.UpdateFOV(player.GridPosition);
			}
		}
	}

	/// <summary>
    /// Executa um turno para cada ator no mapa.
    /// </summary>
	private void HandleEnemyTurns() {
		foreach (Actor actor in Map.Map_Data.Actors) {
			if (actor is Player) continue;
			// Se o ator for um inimigo e estiver vivo, deixamos
			// que sua IA faça um turno.
			if (actor is Enemy enemy && enemy.IsAlive) {
				enemy.Soul.Perform();
			}
		}
	}
}
