using Godot;
using System;

public abstract partial class DirectionalAction : Action
{
	public Vector2I Offset { get; private set; }
	public DirectionalAction(Actor actor, Vector2I offset) : base(actor)
	{
		Offset = offset;
	}

	protected Actor GetBlockingActorAtPosition(Vector2I pos) {
		return Map_Data.GetBlockingActorAtPosition(pos);
	}
}
