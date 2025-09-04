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
		// Eu te disse que este método seria útil.
		Entity potentialTarget = GetTarget();

		// Só podemos atacar atores.
		if (potentialTarget is not Actor) {
			return;
		}

		Actor target = (Actor)potentialTarget;


		// Se não houver um ator na direção, não podemos continuar.
		// Isto é uma ação gratuita.
		if (target == null) return;

		// não podemos ter dano negativo.
		int damage = actor.Atk - target.Def;

		string attackDesc = $"{actor.DisplayName} ataca {target.DisplayName}";

		if (damage > 0) {
			attackDesc += $" e remove {damage} de HP.";
			target.Hp -= damage;
		} else {
			attackDesc += $" mas {target.DisplayName} tem músculos de aço.";
		}

		MessageLogData.Instance.AddMessage(attackDesc);
		actor.Energy -= cost;
	}
}
