using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Magic;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Actions;

/// <summary>
/// Ação para quando o jogador joga um feitiço.
/// </summary>
public partial class SpellAction : DirectionalAction
{
	private SpellResource spell;

	public SpellAction(Actor actor, Vector2I offset, SpellResource spell) : base(actor, offset)
	{
		this.spell = spell;

		Cost = 5;
	}

	public override bool Perform()
	{
		Actor target;

		if (spell.Type == SpellType.Self)
		{
			target = Actor;
		}
		else if (GetTarget() is Actor actor)
		{
			target = actor;
		}
		else
		{
			return false;
		}

		if (Grid.Distance(Actor.GridPosition, target.GridPosition) > spell.Range)
		{
			return false;
		}

		if (Actor.Mp < spell.Cost)
		{
			return false;
		}

		foreach (SpellEffect effect in spell.Effects)
		{
			effect.Apply(Actor, target);
		}

		Actor.Energy -= Cost;
		return true;
	}
}
