using Godot;
using System;

public partial class InputHandler : Node {
	public Action GetAction() {
		Action action = null;

		if (Input.IsActionJustPressed("walk-up")) {
			action = new BumpAction(Vector2I.Up);
		} else if (Input.IsActionJustPressed("walk-down")) {
			action = new BumpAction(Vector2I.Down);
		} else if (Input.IsActionJustPressed("walk-left")) {
			action = new BumpAction(Vector2I.Left);
		} else if (Input.IsActionJustPressed("walk-right")) {
			action = new BumpAction(Vector2I.Right);
		} else if (Input.IsActionJustPressed("skip-turn")) {
			action = new BumpAction(Vector2I.Zero);
		}

		return action;
	}
}
