using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Magic;

/// <summary>
/// Aplica um efeito de feitiço ao usuário.
/// </summary>
[GlobalClass]
public partial class SpellEffectItemActivation : ItemActivation
{
	[Export]
	SpellEffect effect;

	public override bool OnActivation(Player consumer)
	{
		effect.Apply(consumer, consumer);
		return true;
	}
}