using Godot;

/// <summary>
/// Ação de movimento. Movimenta o ator para a direção de seu Offset.
/// </summary>
public partial class MovementAction : DirectionalAction
{
	public MovementAction(Actor actor, Vector2I offset) : base(actor, offset)
	{
	}

	public override void Perform()
	{
		// Não anda se o destino for um tile sólido.
		if (!Map_Data.IsTileWalkable(Destination)) return;

		// Não anda se o destino for oculpado por um ator.
		// Na maioria dos casos, essa condição nunca é verdadeira.
		if (GetTargetActor() != null) return;

		actor.Energy -= cost;
		actor.Walk(Offset);
	}
}
