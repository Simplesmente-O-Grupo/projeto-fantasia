using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Magic;

[GlobalClass]
public partial class HealEffect : SpellEffect
{
	[Export]
	public int Healing { get; set; }

	public override void Apply(Actor caster, Actor target)
	{
		int prevHealth = target.Hp;
		target.Hp += Healing;
		int healingDealt = target.Hp - prevHealth;
		MessageLogData.Instance.AddMessage($"{caster.DisplayName} restaurou {healingDealt} de HP de {target.DisplayName}");
	}
}