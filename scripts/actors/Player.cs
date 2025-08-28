using Godot;
using System;

/// <summary>
/// Classe do jogador. Por enquanto não é diferente do Ator, mas isso pode mudar.
/// </summary>
[GlobalClass]
public partial class Player : Actor
{
	public Player(Vector2I initialPosition, MapData map, ActorDefinition definition) : base(initialPosition, map, definition)
	{
	}
}
