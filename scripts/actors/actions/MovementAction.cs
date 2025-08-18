using Godot;
using System;

public partial class MovementAction : DirectionalAction
{
	public MovementAction(Actor actor, Vector2I offset) : base(actor, offset)
	{
	}

	public override void Perform()
	{
		Vector2I finalDestination = actor.GridPosition + Offset;

		if (!Map_Data.IsTileWalkable(finalDestination)) return;

		if (GetBlockingActorAtPosition(finalDestination) != null) return;

		actor.Walk(Offset);
	}
}
