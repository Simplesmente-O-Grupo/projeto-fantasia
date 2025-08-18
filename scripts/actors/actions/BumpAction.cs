using Godot;
using System;

public partial class BumpAction : DirectionalAction
{
	public BumpAction(Actor actor, Vector2I offset) : base(actor, offset)
	{
	}

	public override void Perform()
	{
		Vector2I destination = actor.GridPosition + Offset;

		Action action;

		if (GetBlockingActorAtPosition(destination) != null) {
			action = new MeleeAction(actor, Offset);
		} else {
			action = new MovementAction(actor, Offset);
		}

		action.Perform();
	}
}
