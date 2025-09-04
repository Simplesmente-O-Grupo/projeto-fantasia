using Godot;

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
		if (!Map_Data.GetTile(Destination).IsWalkable) return true;

		// Não anda se o destino for oculpado por um ator.
		// Na maioria dos casos, essa condição nunca é verdadeira.
		if (GetTarget() != null) return true;

		actor.Walk(Offset);
		actor.Energy -= cost;

		return true;
	}
}
