using Godot;
using System;

public partial class MovementAction : DirectionalAction
{
	public MovementAction(Vector2I offset) : base(offset)
	{
	}

	public override void Perform(Game game, Actor actor)
	{
		Vector2I finalDestination = actor.GridPosition + Offset;

		if (!game.Map.IsTileWalkable(finalDestination)) return;

		if (game.Map.GetBlockingActorAtPosition(finalDestination) != null) return;

		actor.Walk(Offset);
	}
}
