using Godot;
using System;

/// <summary>
/// Classe do jogador. Por enquanto não é diferente do Ator, mas isso pode mudar.
/// </summary>
[GlobalClass]
public partial class Player : Actor
{
	private PlayerDefinition definition;
	public Inventory inventory;

	public Player(Vector2I initialPosition, MapData map, PlayerDefinition definition) : base(initialPosition, map, definition)
	{
		this.definition = definition;
		SetDefinition(definition);
	}

	public void SetDefinition(PlayerDefinition definition) {
		inventory = new(definition.InventoryCapacity);

		AddChild(inventory);
	}
}
