using Godot;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.Magic;

[GlobalClass]
public partial class DamageEffect : SpellEffect
{
	[Export]
	public int Damage { get; set; }

	public override void Apply(Actor caster, Actor target)
	{
		int damageDealt = Damage - target.Men;

		target.Hp -= damageDealt;
	}
}