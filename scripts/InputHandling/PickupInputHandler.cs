using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.InputHandling;

/// <summary>
/// Esquema de controles para pegar um item.
/// </summary>
public partial class PickupInputHandler : BaseInputHandler
{
	private readonly Godot.Collections.Dictionary<string, Vector2I> directions = new()
	{
		{"walk-up", Vector2I.Up},
		{"walk-down", Vector2I.Down},
		{"walk-left", Vector2I.Left},
		{"walk-right", Vector2I.Right},
		{"walk-up-right", Vector2I.Up + Vector2I.Right},
		{"walk-up-left", Vector2I.Up + Vector2I.Left},
		{"walk-down-right", Vector2I.Down + Vector2I.Right},
		{"walk-down-left", Vector2I.Down + Vector2I.Left},
		{"skip-turn", Vector2I.Zero}
	};

	public override Action GetAction(Player player)
	{
		Action action = null;

		foreach (var direction in directions)
		{
			if (Input.IsActionJustPressed(direction.Key))
			{
				action = new PickupAction(player, direction.Value);
				Quit();
			}
		}

		if (Input.IsActionJustPressed("quit"))
		{
			Quit();
		}

		return action;
	}

	private void Quit()
	{
		GetParent<InputHandler>().SetInputHandler(InputHandlers.MainGame);
	}
}
