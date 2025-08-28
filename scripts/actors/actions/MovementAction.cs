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
		Vector2I finalDestination = actor.GridPosition + Offset;

		// Não anda se o destino for um tile sólido.
		if (!Map_Data.IsTileWalkable(finalDestination)) return;

		// Não anda se o destino for oculpado por um ator.
		// Na maioria dos casos, essa condição nunca é verdadeira.
		if (GetBlockingActorAtPosition(finalDestination) != null) return;

		actor.Walk(Offset);
	}
}
