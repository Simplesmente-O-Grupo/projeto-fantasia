using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.GUI;
using TheLegendOfGustav.Magic;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.InputHandling;

public partial class SpellMenuInputHandler : BaseInputHandler
{
	private static readonly PackedScene spellMenuScene = GD.Load<PackedScene>("res://scenes/GUI/spellbook_menu.tscn");

	[Export]
	private Map.Map map;

	private SpellBookMenu spellBookMenu;
	private SpellResource spellCast = null;

	public override void Enter()
	{
		spellBookMenu = spellMenuScene.Instantiate<SpellBookMenu>();
		map.MapData.Player.AddChild(spellBookMenu);
		spellBookMenu.Initialize(map.MapData.Player.SpellBook);
		spellBookMenu.SpellSelected += OnSpellCast;
	}

	public override void Exit()
	{
		spellCast = null;
		spellBookMenu.QueueFree();
	}

	public override Action GetAction(Player player)
	{
		Action action = null;

		if (spellCast != null)
		{
			if (spellCast.Type == SpellType.Ranged)
			{
				SignalBus.Instance.EmitSignal(SignalBus.SignalName.PlayerSpellChooseLocation, spellCast);
				GetParent<InputHandler>().SetInputHandler(InputHandlers.CastSpell);
				return action;
			}
			action = new SpellAction(player, Vector2I.Zero, spellCast);
			Close();
			return action;
		}

		if (Input.IsActionJustPressed("quit"))
		{
			Close();
		}

		return action;
	}

	private void Close()
	{
		GetParent<InputHandler>().SetInputHandler(InputHandlers.MainGame);
	}

	private void OnSpellCast(SpellResource spell)
	{
		spellCast = spell;
	}
}