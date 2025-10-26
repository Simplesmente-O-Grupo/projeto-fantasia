using Godot;
using Godot.Collections;

namespace TheLegendOfGustav.Magic;

public partial class SpellBook : Node, ISaveable
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

	public Dictionary<string, Variant> GetSaveData()
	{
		Array<string> spellPaths = [];
		foreach(SpellResource spell in KnownSpells)
		{
			spellPaths.Add(spell.ResourcePath);
		}

		return new()
		{
			{"spells", spellPaths}
		};
	}

	public bool LoadSaveData(Dictionary<string, Variant> saveData)
	{
		Array<string> paths = (Array<string>)saveData["spells"];

		foreach(string path in paths)
		{
			SpellResource spell = GD.Load<SpellResource>(path);
			KnownSpells.Add(spell);
		}

		return true;
	}
}