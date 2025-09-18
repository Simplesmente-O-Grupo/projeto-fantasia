using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Actions;

/// <summary>
/// Ação de movimento. Movimenta o ator para a direção de seu Offset.
/// </summary>
public partial class MovementAction : DirectionalAction
{
	public MovementAction(Actor actor, Vector2I offset) : base(actor, offset)
	{
	}

	public override bool Perform()
	{
		// Não anda se o destino for um tile sólido.
		if (!MapData.GetTile(Destination).IsWalkable) return true;

		// Não anda se o destino for oculpado por um ator.
		// Na maioria dos casos, essa condição nunca é verdadeira.
		if (GetTarget() != null) return true;

		Actor.Walk(Offset);
		Actor.Energy -= cost;

		return true;
	}
}
