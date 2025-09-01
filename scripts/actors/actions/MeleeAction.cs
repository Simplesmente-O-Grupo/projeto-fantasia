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
		actor.Energy -= cost;
		// Eu te disse que este método seria útil.
		Actor target = GetTargetActor();

		// Se não houver um ator na direção, não podemos continuar.
		if (target == null) return;

		// não podemos ter dano negativo.
		int damage = actor.Atk - target.Def;

		string attackDesc = $"{actor.ActorName} ataca {target.ActorName}";

		if (damage > 0) {
			attackDesc += $" e remove {damage} de HP.";
			target.Hp -= damage;
		} else {
			attackDesc += $" mas {target.ActorName} tem músculos de aço.";
		}

		MessageLogData.Instance.AddMessage(attackDesc);
	}
}
