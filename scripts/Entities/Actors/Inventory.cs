using Godot;
using TheLegendOfGustav.Entities.Items;
using TheLegendOfGustav.Map;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Actors;

public partial class Inventory(int capacity) : Node
{
	private Player player;
	public int Capacity { get; private set; } = capacity;
	public Godot.Collections.Array<ConsumableItem> Items { get; private set; } = [];

	public override void _Ready()
	{
		base._Ready();
		player = GetParent<Player>();
	}

	public void Drop(ConsumableItem item)
	{
		Items.Remove(item);

		MapData data = player.MapData;
		
		data.InsertEntity(item);
		data.EmitSignal(MapData.SignalName.EntityPlaced, item);
		
		item.MapData = data;
		item.GridPosition = player.GridPosition;

		MessageLogData.Instance.AddMessage($"Você descarta {item.DisplayName}.");
	}

	public void Add(ConsumableItem item)
	{
		if (Items.Count >= Capacity) return;

		Items.Add(item);
	}

	public void RemoveItem(ConsumableItem item)
	{
		Items.Remove(item);
	}
}