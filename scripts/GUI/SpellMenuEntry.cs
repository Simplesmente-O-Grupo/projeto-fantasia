using Godot;
using System;
using TheLegendOfGustav.Magic;

namespace TheLegendOfGustav.GUI;

public partial class SpellMenuEntry : HBoxContainer
{
	private TextureRect icon;
	private Label shortcutLabel;
	private Label nameLabel;
	private Button castBtn;
	private SpellResource spell;

	[Signal]
	public delegate void CastEventHandler(SpellResource Item);

	public override void _Ready()
	{
		base._Ready();
		icon = GetNode<TextureRect>("Icon");
		shortcutLabel = GetNode<Label>("Shortcut");
		nameLabel = GetNode<Label>("SpellName");
		castBtn = GetNode<Button>("CastButton");

		castBtn.Pressed += () => EmitSignal(SignalName.Cast, spell);
	}

	public void Initialize(SpellResource spell, char? shortcut)
	{
		this.spell = spell;
		nameLabel.Text = spell.SpellName;
		if (shortcut != null)
		{
			shortcutLabel.Text = $"{shortcut}";
			
			int index = (int)shortcut - 'a';

			InputEventKey activateEvent = new()
			{
				Keycode = Key.A + index
			};

			Shortcut shortie = new()
			{
				Events = [activateEvent]
			};

			castBtn.Shortcut = shortie;
		}
		else
		{
			shortcutLabel.Text = "";
		}
		icon.Texture = spell.Icon;
	}
}
