using Godot;
using TheLegendOfGustav.Entities.Items;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.GUI;

public partial class InventoryMenu : CanvasLayer
{
	private static readonly PackedScene itemMenuEntryScene = GD.Load<PackedScene>("res://scenes/GUI/item_menu_entry.tscn");
	
	private VBoxContainer itemsNode;
	
	[Signal]
	public delegate void ItemSelectedEventHandler(ConsumableItem item);
	[Signal]
	public delegate void ItemDropEventHandler(ConsumableItem item);

	public override void _Ready()
	{
		base._Ready();

		itemsNode = GetNode<VBoxContainer>("CenterContainer/PanelContainer/VBoxContainer/Items");
		Hide();
	}

	public void OnActivate(ConsumableItem item)
	{
		EmitSignal(SignalName.ItemSelected, item);
	}

	public void OnDrop(ConsumableItem item)
	{
		EmitSignal(SignalName.ItemDrop, item);
	}

	public void Initialize(Inventory inventory)
	{
		for (int i = 0; i < inventory.Items.Count; i++)
		{
			RegisterItem(i, inventory.Items[i]);
		}

		Show();
	}
	
	private void RegisterItem(int index, ConsumableItem item)
	{
		char? shortcut = null;

		// Só terá atalho para as letras do alfabeto.
		if (index < 26)
		{
			shortcut = (char)('a' + index);
		}

		ItemMenuEntry itemEntry = itemMenuEntryScene.Instantiate<ItemMenuEntry>();

		itemsNode.AddChild(itemEntry);
		itemEntry.Initialize(item, shortcut);
		itemEntry.Activate += OnActivate;
		itemEntry.Drop += OnDrop;
	}
}
