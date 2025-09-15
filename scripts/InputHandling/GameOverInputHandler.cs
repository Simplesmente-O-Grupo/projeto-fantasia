using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.InputHandling;

/// <summary>
/// Esquema de controles para quando o jogador está morto.
/// </summary>
public partial class GameOverInputHandler : BaseInputHandler
{
	// Por enquanto não tem nada.
	public override Action GetAction(Player player)
	{
		Action action = null;
		if (Input.IsActionJustPressed("quit"))
		{
			action = new EscapeAction(player);
		}
		return action;
	}
}