using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.InputHandling;

/// <summary>
/// Esquema de controles principal do jogo.
/// </summary>
public partial class MainGameInputHandler : BaseInputHandler
{
	private static readonly Godot.Collections.Dictionary<string, Vector2I> directions = new()
	{
		{"walk-up", Vector2I.Up},
		{"walk-down", Vector2I.Down},
		{"walk-left", Vector2I.Left},
		{"walk-right", Vector2I.Right},
		{"walk-up-right", Vector2I.Up + Vector2I.Right},
		{"walk-up-left", Vector2I.Up + Vector2I.Left},
		{"walk-down-right", Vector2I.Down + Vector2I.Right},
		{"walk-down-left", Vector2I.Down + Vector2I.Left},
	};

	public override Action GetAction(Player player)
	{
		Action action = null;

		foreach (var direction in directions)
		{
			if (Input.IsActionJustPressed(direction.Key))
			{
				action = new BumpAction(player, direction.Value);
			}
		}

		if (Input.IsActionJustPressed("open-inventory"))
		{
			GetParent<InputHandler>().SetInputHandler(InputHandlers.Inventory);
		}

		if (Input.IsActionJustPressed("pick-item"))
		{
			GetParent<InputHandler>().SetInputHandler(InputHandlers.Pickup);
		}

		if (Input.IsActionJustPressed("inspect"))
		{
			GetParent<InputHandler>().SetInputHandler(InputHandlers.Inspect);
		}

		if (Input.IsActionJustPressed("open-spellbook"))
		{
			GetParent<InputHandler>().SetInputHandler(InputHandlers.SpellMenu);
		}

		if (Input.IsActionJustPressed("skip-turn"))
		{
			action = new WaitAction(player);
		}

		if (Input.IsActionJustPressed("quit"))
		{
			action = new EscapeAction(player);
		}

		return action;
	}
}
