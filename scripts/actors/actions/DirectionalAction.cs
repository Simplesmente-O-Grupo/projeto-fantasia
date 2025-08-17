using Godot;
using System;

public abstract partial class DirectionalAction : Action
{
	public Vector2I Offset { get; private set; }
	public DirectionalAction(Vector2I offset)
	{
		Offset = offset;
	}
}
