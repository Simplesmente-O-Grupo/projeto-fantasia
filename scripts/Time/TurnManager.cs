using Godot;
using TheLegendOfGustav.Map;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Time;

/// <summary>
/// Gerenciador de turnos, o senhor do tempo.
/// </summary>
public partial class TurnManager(Map.Map map) : RefCounted
{
	private Map.Map map = map;
	/// <summary>
	/// As ações do jogador ficam em uma fila e são executadas quando
	/// possível dentro das regras do senhor do tempo.
	/// </summary>
	private Godot.Collections.Array<Action> playerActionQueue = [];

	/// <summary>
    /// A quantidade de turnos que se passaram.
    /// </summary>
	public int TurnCount { get; private set; } = 0;

	private MapData Map_Data { get => map.MapData; }
	private Player Player { get => Map_Data.Player; }

	/// <summary>
	/// Insere uma ação na fila de ações do jogador.
	/// </summary>
	/// <param name="playerAction"></param>
	public void InsertPlayerAction(Action playerAction)
	{
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
	public void Tick()
	{
		// Se o jogador puder agir mas a fila de ações estiver vazia,
		// não computamos o turno.
		if (playerActionQueue.Count == 0 && Player.Energy > 0)
		{
			return;
		}

		Vector2I previousPlayerPos = Player.GridPosition;

		// Início do turno, o jogador recebe um pouco de energia.
		if (Player.Energy <= 0)
		{
			StartTurn();
		}

		bool actionResult = true; ;
		// Primeiro executamos a ação do jogador, se ele puder.
		if (playerActionQueue.Count > 0 && Player.Energy > 0)
		{
			Action action = playerActionQueue[0];
			playerActionQueue.RemoveAt(0);

			actionResult = action.Perform();

			// TODO: Isto é feio, lembre-me de mudar isto antes da entrega final.
			if (action is SpellAction)
			{
				GD.Print(actionResult);
				SignalBus.Instance.EmitSignal(SignalBus.SignalName.PlayerSpellCast, actionResult);
			}
		}

		// Se a ação do jogador for gratuita ou se o jogador ainda possuir energia, 
		// ele poderá fazer mais um turno sem interrupções.
		if (actionResult && Player.Energy <= 0)
		{
			// Depois computamos os turnos dos outros atores.
			HandleEnemyTurns();
			map.UpdateFOV(Player.GridPosition);
		}
		// Por fim, se o jogador mudou de lugar, atualizamos seu campo de visão.
		if (Player.GridPosition != previousPlayerPos)
		{
			map.UpdateFOV(Player.GridPosition);
		}
	}

	/// <summary>
	/// Método executado no início do turno.
	/// </summary>
	private void StartTurn()
	{
		TurnCount++;

		// Recarregamos a energia de todos os atores.
		foreach (Entity entity in Map_Data.Entities)
		{
			if (entity is Actor actor && actor.IsAlive)
			{
				actor.OnTurnStart(TurnCount);
			}
		}
	}

	/// <summary>
	/// Executa turnos para cada ator no mapa.
	/// </summary>
	private void HandleEnemyTurns()
	{
		foreach (Entity entity in Map_Data.Entities)
		{
			if (entity is Player) continue;
			// Se o ator for um inimigo e estiver vivo, deixamos
			// que sua IA faça um turno.
			if (entity is Enemy enemy && enemy.IsAlive)
			{
				// O inimigo poderá fazer quantos turnos sua energia deixar.
				while (enemy.Energy > 0)
				{
					enemy.Soul.Perform();
				}
			}
		}
	}
}
