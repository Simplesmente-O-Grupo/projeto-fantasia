using Godot;
using Godot.Collections;
using TheLegendOfGustav.Entities;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Items;

public partial class ItemEntity : Entity, ISaveable
{
	public Item Item { get; private set; }

	public ItemEntity(Vector2I initialPosition, MapData map, Item item) : base(initialPosition, map)
	{
		Item = item;
		// Eu quero muito reescrever o jogo do zero, mas não tenho tempo :(
		EntityDefinition sad = new()
		{
			blocksMovement = false,
			name = item.Definition.DisplayName,
			texture = item.Definition.Icon,
			Type = EntityType.ITEM,
		};

		SetDefinition(sad);
	}
	public ItemEntity(Vector2I initialPosition, MapData map) : base(initialPosition, map)
	{
	}

	public new Dictionary<string, Variant> GetSaveData()
	{
		Dictionary<string, Variant> baseData = base.GetSaveData();
		baseData.Add("item", Item.GetSaveData());

		return baseData;
	}

	public new bool LoadSaveData(Dictionary<string, Variant> saveData)
	{
		Item = new();

		if (!Item.LoadSaveData((Dictionary<string, Variant>)saveData["item"]))
		{
			return false;
		}
		// Eu quero muito reescrever o jogo do zero, mas não tenho tempo :(
		EntityDefinition sad = new()
		{
			blocksMovement = false,
			name = Item.Definition.DisplayName,
			texture = Item.Definition.Icon,
			Type = EntityType.ITEM,
		};

		SetDefinition(sad);

		base.LoadSaveData(saveData);

		return true;
	}
}