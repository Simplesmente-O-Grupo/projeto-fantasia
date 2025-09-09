using Godot;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.Entities.Actions;

/// <summary>
/// Ação de "Esbarramento", utilizada principalmente pelo jogador.
/// Esta ação direcionada tentará andar para o destino, se houver um
/// ator no caminho, uma ação de ataque é gerada no lugar.
/// </summary>
public partial class BumpAction : DirectionalAction
{
	public BumpAction(Actor actor, Vector2I offset) : base(actor, offset)
	{
	}

	// Como esta ação inevitavelmente gera outras ações,
	// não faz sentido descontar a energia do ator.
	public override bool Perform()
	{
		// Declaramos uma ação genérica.
		Action action;

		// Se houver um ator no destino, crie uma ação de ataque.
		if (GetTarget() != null)
		{
			action = new MeleeAction(Actor, Offset);
		}
		else
		{
			// Mas se não houver, crie uma ação de movimento.
			action = new MovementAction(Actor, Offset);
		}

		// Executa a ação.
		return action.Perform();
	}
}
