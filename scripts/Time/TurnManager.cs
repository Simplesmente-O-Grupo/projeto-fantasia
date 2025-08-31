using Godot;

/// <summary>
/// Gerenciador de turnos, o senhor do tempo.
/// </summary>
public partial class TurnManager : RefCounted
{
	public int TurnCount { get; private set; } = 0;
	private Map map;
	private MapData Map_Data { get => map.Map_Data; }
	private Player Player { get => Map_Data.Player; }

	/// <summary>
    /// As ações do jogador ficam em uma fila e são executadas quando
    /// possível dentro das regras do senhor do tempo.
    /// </summary>
	private Godot.Collections.Array<Action> playerActionQueue = [];

	public TurnManager(Map map) {
		this.map = map;
	}

	/// <summary>
    /// Insere uma ação na fila de ações do jogador.
    /// </summary>
    /// <param name="playerAction"></param>
	public void InsertPlayerAction(Action playerAction) {
		playerActionQueue.Add(playerAction);
	}

	/// <summary>
    /// Computa um turno.
    /// 
    /// Um turno segue a seguinte ordem lógica
    /// 1. Todos os atores recebem energia com base nas suas velocidades.
    /// 2. O jogador performa ações enquanto sua energia permitir
    /// 3. Os outros atores performam suas ações enquanto sua energia permitir.
    /// </summary>
	public void Tick() {
		// Se o jogador puder agir mas a fila de ações estiver vazia,
		// não computamos o turno.
		if (playerActionQueue.Count == 0 && Player.Energy > 0) {
			return;
		}

		Vector2I previousPlayerPos = Player.GridPosition;

		// Início do turno, o jogador recebe um pouco de energia.
		if (Player.Energy <= 0) {
			StartTurn();
		}

		// Primeiro executamos a ação do jogador, se ele puder.
		if (Player.Energy > 0) {
			Action action = playerActionQueue[0];
			playerActionQueue.RemoveAt(0);

			action.Perform();
		}

		// Se o jogador ainda tem energia, ele poderá fazer
		// mais um turno sem interrupções.
		if (Player.Energy <= 0) {
			// Depois computamos os turnos dos outros atores.
			HandleEnemyTurns();
		}
		// Por fim, se o jogador mudou de lugar, atualizamos seu campo de visão.
		if (Player.GridPosition != previousPlayerPos) {
			map.UpdateFOV(Player.GridPosition);
		}
	}

	/// <summary>
    /// Método executado no início do turno.
    /// </summary>
	private void StartTurn() {
		TurnCount++;

		// Recarregamos a energia de todos os atores.
		foreach (Actor actor in Map_Data.Actors) {
			if (actor.IsAlive) {
				actor.RechargeEnergy();
			}
		}
	}

	/// <summary>
	/// Executa turnos para cada ator no mapa.
	/// </summary>
	private void HandleEnemyTurns() {
		foreach (Actor actor in Map_Data.Actors) {
			if (actor is Player) continue;
			// Se o ator for um inimigo e estiver vivo, deixamos
			// que sua IA faça um turno.
			if (actor is Enemy enemy && enemy.IsAlive) {
				// O inimigo poderá fazer quantos turnos sua energia deixar.
				while (enemy.Energy > 0) {
					enemy.Soul.Perform();
				}
			}
		}
	}
}
