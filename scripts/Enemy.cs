using Godot;
using System;

public partial class Enemy : Actor
{
	public Enemy(Vector2I initialPosition, MapData map, ActorDefinition definition) : base(initialPosition, map, definition)
	{
	}
}
