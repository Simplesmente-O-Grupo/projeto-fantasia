using Godot;
using System;

public partial class BumpAction : DirectionalAction
{
	public BumpAction(Vector2I offset) : base(offset)
	{
	}

	public override void Perform(Game game, Actor actor)
	{
		Vector2I destination = actor.GridPosition + Offset;

		Action action;

		if (game.Map.GetBlockingActorAtPosition(destination) != null) {
			action = new MeleeAction(Offset);
		} else {
			action = new MovementAction(Offset);
		}

		action.Perform(game, actor);
	}
}
