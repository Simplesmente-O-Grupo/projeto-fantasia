using Godot;
using System;
using TheLegendOfGustav.Magic;

namespace TheLegendOfGustav.GUI;

public partial class SpellBookMenu : CanvasLayer
{
	private static readonly PackedScene spellMenuEntryScene = GD.Load<PackedScene>("res://scenes/GUI/spell_menu_entry.tscn");

	private VBoxContainer spellsNode;

	[Signal]
	public delegate void SpellSelectedEventHandler(SpellResource spell);

	public override void _Ready()
	{
		base._Ready();

		spellsNode = GetNode<VBoxContainer>("CenterContainer/PanelContainer/VBoxContainer/Spells");
		Hide();
	}

	public void OnCast(SpellResource spell)
	{
		EmitSignal(SignalName.SpellSelected, spell);
	}

	public void Initialize(SpellBook spellBook)
	{
		for (int i = 0; i < spellBook.KnownSpells.Count; i++)
		{
			RegisterSpell(i, spellBook.KnownSpells[i]);
		}

		Show();
	}

	private void RegisterSpell(int index, SpellResource spell)
	{
		char? shortcut = null;

		// Só terá atalho para as letras do alfabeto.
		if (index < 26)
		{
			shortcut = (char)('a' + index);
		}

		SpellMenuEntry spellEntry = spellMenuEntryScene.Instantiate<SpellMenuEntry>();

		spellsNode.AddChild(spellEntry);
		spellEntry.Initialize(spell, shortcut);
		spellEntry.Cast += OnCast;
	}
}
