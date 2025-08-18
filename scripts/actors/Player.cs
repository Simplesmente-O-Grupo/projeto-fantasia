using Godot;
using System;

[GlobalClass]
public partial class Player : Actor
{
	public Player(Vector2I initialPosition, DungeonLevel map, ActorDefinition definition) : base(initialPosition, map, definition)
	{
	}
}
