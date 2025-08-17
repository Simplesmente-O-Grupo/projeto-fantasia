using Godot;
using System;

public partial class InputHandler : Node {
	public Action GetAction() {
		Action action = null;

		if (Input.IsActionJustPressed("walk-up")) {
			action = new MovementAction(Vector2I.Up);
		} else if (Input.IsActionJustPressed("walk-down")) {
			action = new MovementAction(Vector2I.Down);
		} else if (Input.IsActionJustPressed("walk-left")) {
			action = new MovementAction(Vector2I.Left);
		} else if (Input.IsActionJustPressed("walk-right")) {
			action = new MovementAction(Vector2I.Right);
		} else if (Input.IsActionJustPressed("skip-turn")) {
			action = new MovementAction(Vector2I.Zero);
		}

		return action;
	}
}
