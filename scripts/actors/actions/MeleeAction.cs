using Godot;

/// <summary>
/// Ação de ataque físico. Uma ação direcionada que ataca um alvo.
/// </summary>
public partial class MeleeAction : DirectionalAction
{
	public MeleeAction(Actor actor, Vector2I offset) : base(actor, offset)
	{
	}

	/// <summary>
	/// Ataca o ator na direção da ação.
	/// </summary>
	public override void Perform()
	{
		Vector2I destination = actor.GridPosition + Offset;
		// Eu te disse que este método seria útil.
		Actor target = GetBlockingActorAtPosition(destination);

		// Se não houver um ator na direção, não podemos continuar.
		if (target == null) return;

		// TODO: Implementar ataque.
		GD.Print($"Você tenta socar {target.ActorName}, mas como não sobra nada para o beta, você ainda não tem um método de ataque.");
	}
}
