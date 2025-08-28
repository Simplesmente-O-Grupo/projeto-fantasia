using System.Numerics;
using Godot;

/// <summary>
/// Obtém input do usuário.
/// </summary>
public partial class InputHandler : Node {
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
	};
	public Action GetAction(Player player) {
		Action action = null;

		foreach (var direction in directions) {
			if (Input.IsActionJustPressed(direction.Key)) {
				action = new BumpAction(player, direction.Value);
			}
		}
		
		if (Input.IsActionJustPressed("skip-turn")) {
			action = new WaitAction(player);
		}

		return action;
	}
}
