using Godot;

namespace TheLegendOfGustav.Magic;

public partial class SpellBook : Node
{
	public Godot.Collections.Array<SpellResource> KnownSpells { get; private set; } = [];
	
	public bool KnowsSpell(SpellResource spell) => KnownSpells.Contains(spell);

	public void LearnSpell(SpellResource spell) {
		if (!KnownSpells.Contains(spell)) {
			KnownSpells.Add(spell);
		}
	}

	public void ForgetSpell(SpellResource spell) {
		KnownSpells.Remove(spell);
	}
}