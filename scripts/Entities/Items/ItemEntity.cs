using Godot;
using TheLegendOfGustav.Entities;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Items;

public partial class ItemEntity : Entity
{
	public Item Item { get; private set; }

	public ItemEntity(Vector2I initialPosition, MapData map, Item item) : base(initialPosition, map)
	{
		Item = item;
		// Eu quero muito reescrever o jogo do zero, mas não tenho tempo :(
		EntityDefinition sad = new()
		{
			blocksMovement = true,
			name = item.Definition.DisplayName,
			texture = item.Definition.Icon,
			Type = EntityType.ITEM,
		};

		SetDefinition(sad);
	}
}