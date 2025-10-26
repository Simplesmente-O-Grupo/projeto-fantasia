using Godot;
using Godot.Collections;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Actors;

/// <summary>
/// Classe do jogador. Por enquanto não é diferente do Ator, mas isso pode mudar.
/// </summary>
[GlobalClass]
public partial class Player : Actor, ISaveable
{
	private PlayerDefinition definition;

	public Player(Vector2I initialPosition, MapData map, PlayerDefinition definition) : base(initialPosition, map, definition)
	{
		this.definition = definition;
		SetDefinition(definition);
	}

	public Inventory Inventory { get; private set; }

	public new Dictionary<string, Variant> GetSaveData()
	{
		Dictionary<string, Variant> baseData = base.GetSaveData();
		baseData.Add("inventory", Inventory.GetSaveData());
		baseData.Add("definition", definition.ResourcePath);

		return baseData;
	}

	public new bool LoadSaveData(Dictionary<string, Variant> saveData)
	{
		PlayerDefinition definition = GD.Load<PlayerDefinition>((string)saveData["definition"]);

		SetDefinition(definition);

		if (!base.LoadSaveData(saveData))
		{
			return false;
		}

		if(!Inventory.LoadSaveData((Dictionary<string, Variant>)saveData["inventory"]))
		{
			return false;
		}

		return true;
	}

	public void SetDefinition(PlayerDefinition definition)
	{
		Inventory?.QueueFree();
		Inventory = new(definition.InventoryCapacity);

		AddChild(Inventory);
	}
}
