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

		if (damageDealt < 0)
		{
			damageDealt = 0;
		}

		if (damageDealt <= 0) {
			MessageLogData.Instance.AddMessage($"{target.DisplayName} resite o ataque mágico de {caster.DisplayName}");
		}
		else {
			MessageLogData.Instance.AddMessage($"{caster.DisplayName} aplica {damageDealt} de dano mágico em {target.DisplayName}");
		}

		target.Hp -= damageDealt;
	}
}