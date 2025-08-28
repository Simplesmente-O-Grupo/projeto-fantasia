using Godot;

/// <summary>
/// Obtém input do usuário.
/// </summary>
public partial class InputHandler : Node {
	public Action GetAction(Player player) {
		Action action = null;

		if (Input.IsActionJustPressed("walk-up")) {
			action = new BumpAction(player, Vector2I.Up);
		} else if (Input.IsActionJustPressed("walk-down")) {
			action = new BumpAction(player, Vector2I.Down);
		} else if (Input.IsActionJustPressed("walk-left")) {
			action = new BumpAction(player, Vector2I.Left);
		} else if (Input.IsActionJustPressed("walk-right")) {
			action = new BumpAction(player, Vector2I.Right);
		} else if (Input.IsActionJustPressed("skip-turn")) {
			action = new BumpAction(player, Vector2I.Zero);
		}

		return action;
	}
}
