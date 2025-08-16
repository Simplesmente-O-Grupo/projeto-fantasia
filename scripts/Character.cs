using Godot;
using System;

public partial class Character : Actor {
	private bool canAct = false;

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (!@event.IsPressed()) return;

		if (canAct) {
			

			if (@event.IsActionPressed("walk-up")) {
				Walk(Vector2I.Up);
			}
			if (@event.IsActionPressed("walk-down")) {
				Walk(Vector2I.Down);
			}
			if (@event.IsActionPressed("walk-left")) {
				Walk(Vector2I.Left);
			}
			if (@event.IsActionPressed("walk-right")) {
				GD.Print("Hello!");
				Walk(Vector2I.Right);
			}

			if (@event.IsActionPressed("skip-turn")) {
				SkipTurn();
			}
		}
	}

	private void SkipTurn() {
		GD.Print("Skipped the turn.");
		Energy = 0;
		EndAction();
	}

	protected override void EndAction() {
		canAct = false;
		base.EndAction();
	}

	public override void performAction() {
		GD.Print("I can act");
		canAct = true;
	}
}
