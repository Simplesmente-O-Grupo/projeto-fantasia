using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Magic;

namespace TheLegendOfGustav.Entities.Actions;

public partial class SpellCommand(Actor actor, Vector2I offset, SpellResource spell) : DirectionalAction(actor, offset)
{
	private SpellResource spell = spell;

	public override bool Perform()
	{
		Actor target = null;

		if (GetTarget() is Actor actor)
		{
			target = actor;
		}

		if (spell.Type == SpellType.Ranged && target == null) return false;

		if (Actor.Mp < spell.Cost)
		{
			return false;
		}

		foreach (SpellEffect effect in spell.Effects)
		{
			effect.Apply(Actor, target);
		}
		return true;
	}
}
