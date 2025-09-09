using Godot;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Actors;

/// <summary>
/// Classe do jogador. Por enquanto não é diferente do Ator, mas isso pode mudar.
/// </summary>
[GlobalClass]
public partial class Player : Actor
{
	public Player(Vector2I initialPosition, MapData map, PlayerDefinition definition) : base(initialPosition, map, definition)
	{
		Definition = definition;
		SetDefinition(definition);
	}

	private PlayerDefinition Definition { get; set; }
	public Inventory Inventory { get; private set; }

	public void SetDefinition(PlayerDefinition definition)
	{
		Inventory = new(definition.InventoryCapacity);

		AddChild(Inventory);
	}
}
