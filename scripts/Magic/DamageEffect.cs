using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Magic;

[GlobalClass]
public partial class DamageEffect : SpellEffect
{
	[Export]
	public int Damage { get; set; }

	public override void Apply(Actor caster, Actor target)
	{
		int damageDealt = Damage - target.Men;

		MessageLogData.Instance.AddMessage($"{caster.DisplayName} aplica {damageDealt} de dano mágico em {target.DisplayName}");

		target.Hp -= damageDealt;
	}
}