using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Actors.AI;

/// <summary>
/// Uma IA simples. Sempre tentará atacar o jogador com ataques corpo a corpo.
/// </summary>
public partial class HostileEnemyAI : BaseAI
{
	/// <summary>
	/// Caminho até a última posição conhecida do jogador.
	/// </summary>
	private Godot.Collections.Array<Vector2> path = [];

	public override void Perform()
	{
		// O alvo da IA sempre é o jogador.
		Player target = body.MapData.Player;
		// Vetor que parte do inimigo até o jogador.
		Vector2I offset = target.GridPosition - body.GridPosition;
		// Distância entre o inimigo e o jogador. Leva em consideração somente
		// um dos eixos.
		int distance = Grid.Distance(body.GridPosition, target.GridPosition);

		// A ação executada no turno pode ser de ataque ou de movimento.
		Action action;

		// Só faz sentido atacar o jogador se o inimigo estiver visível.
		if (body.MapData.GetTile(body.GridPosition).IsInView)
		{
			// Se o inimigo consegue ver que o jogador está morto,
			// IT'S OVER.
			if (!target.IsAlive)
			{
				action = new WaitAction(body);
				action.Perform();
				return;
			}

			// Se estiver do lado do jogador, ataque.
			if (distance <= 1)
			{
				action = new MeleeAction(body, offset);
				action.Perform();
				// Executada a ação, acabamos nosso turno aqui.
				return;
			}

			// Se o inimigo estiver visível para o jogador,
			// consideramos que ele também consiga ver o jogador.
			// Logo, atualizamos o caminho para a posição atual do jogador.
			path = GetPathTo(target.GridPosition);
			// O primeiro passo é a posição atual do inimigo, podemos remover.
			path.RemoveAt(0);
		}

		// Se existir um caminho conhecido para o jogador.
		if (path.Count > 0)
		{
			// Pegamos o próximo passo para o destino.
			Vector2I destination = (Vector2I)path[0];
			// Se tiver o caminho estiver bloqueado, paramos o nosso turno aqui.
			if (body.MapData.GetBlockingEntityAtPosition(destination) != null)
			{
				action = new WaitAction(body);
				action.Perform();
				return;
			}

			// Caso o contrário, criamos uma nova ação de movimentação e a executamos.
			action = new MovementAction(body, destination - body.GridPosition);
			action.Perform();
			// Podemos remover o passo do caminho.
			path.RemoveAt(0);
			return;
		}

		// Senão, espere.
		action = new WaitAction(body);
		action.Perform();
		return;
	}
}