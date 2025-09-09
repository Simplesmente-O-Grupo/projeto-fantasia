using Godot;

public partial class Inventory : Node {
	private Player player;

	public Godot.Collections.Array<ConsumableItem> Items { get; private set; } = [];

	public int Capacity { get; private set; }

	public Inventory(int capacity) {
		Capacity = capacity;
	}

	public override void _Ready() {
		base._Ready();
		player = GetParent<Player>();
	}

	public void Drop(ConsumableItem item) {
		Items.Remove(item);
		MapData data = player.Map_Data;
		data.InsertEntity(item);
		data.EmitSignal(MapData.SignalName.EntityPlaced, item);
		item.Map_Data = data;
		item.GridPosition = player.GridPosition;

		MessageLogData.Instance.AddMessage($"Você descarta {item.DisplayName}.");
	}

	public void Add(ConsumableItem item) {
		if (Items.Count >= Capacity) return;

		Items.Add(item);
	}

	public void RemoveItem(ConsumableItem item) {
		Items.Remove(item);
	}
}