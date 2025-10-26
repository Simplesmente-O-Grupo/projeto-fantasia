using Godot;
using Godot.Collections;
using TheLegendOfGustav.Entities.Items;
using TheLegendOfGustav.Map;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Actors;

public partial class Inventory(int capacity) : Node, ISaveable
{
	private Player player;
	public int Capacity { get; private set; } = capacity;
	public Godot.Collections.Array<Item> Items { get; private set; } = [];

	public override void _Ready()
	{
		base._Ready();
		player = GetParent<Player>();
	}

	public void Drop(Item item)
	{
		Items.Remove(item);

		MapData data = player.MapData;

		ItemEntity itemEnt = new(player.GridPosition, data, item);

		data.InsertEntity(itemEnt);
		data.EmitSignal(MapData.SignalName.EntityPlaced, itemEnt);
		
		MessageLogData.Instance.AddMessage($"Você descarta {item.Definition.DisplayName}.");
	}

	public void Add(Item item)
	{
		if (Items.Count >= Capacity) return;

		Items.Add(item);
	}

	public void RemoveItem(Item item)
	{
		Items.Remove(item);
	}

	public Dictionary<string, Variant> GetSaveData()
	{
		Godot.Collections.Array<Dictionary<string, Variant>> itemsData = [];
		foreach (Item item in Items) {
			itemsData.Add(item.GetSaveData());
		}

		return new()
		{
			{"items", itemsData}
		};
	}

	public bool LoadSaveData(Dictionary<string, Variant> saveData)
	{
		Array<Dictionary<string, Variant>> itemRess = (Array<Dictionary<string, Variant>>)saveData["items"];

		foreach(Dictionary<string, Variant> item in itemRess)
		{
			Item it = new();
			if(!it.LoadSaveData(item))
			{
				return false;
			}

			Items.Add(it);
		}

		return true;
	}
}