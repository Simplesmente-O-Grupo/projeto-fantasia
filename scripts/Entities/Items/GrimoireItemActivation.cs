using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Magic;

/// <summary>
/// Aplica um efeito de feitiço ao usuário.
/// </summary>
[GlobalClass]
public partial class GrimoireItemActivation : ItemActivation
{
	[Export]
	SpellResource spell;

	public override bool OnActivation(Player consumer)
	{
		consumer.SpellBook.LearnSpell(spell);
		return true;
	}
}
