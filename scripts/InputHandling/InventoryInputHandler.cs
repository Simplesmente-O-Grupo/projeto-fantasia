using Godot;
using TheLegendOfGustav.GUI;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities.Items;
using TheLegendOfGustav.Entities.Actions;

namespace TheLegendOfGustav.InputHandling;

public partial class InventoryInputHandler : BaseInputHandler
{
	private static readonly PackedScene inventoryScene = GD.Load<PackedScene>("res://scenes/GUI/invetory_menu.tscn");


	[Export]
	private Map.Map Map { get; set; }

	private InventoryMenu InventoryMenu { get; set; }
	private ConsumableItem ActivationItem { get; set; } = null;
	private ConsumableItem DropItem { get; set; } = null;

	public override void Enter()
	{
		InventoryMenu = inventoryScene.Instantiate<InventoryMenu>();
		Map.MapData.Player.AddChild(InventoryMenu);
		InventoryMenu.Initialize(Map.MapData.Player.Inventory);
		InventoryMenu.ItemSelected += OnItemActivate;
		InventoryMenu.ItemDrop += OnItemDrop;
	}

	public override void Exit()
	{
		ActivationItem = null;
		DropItem = null;
		InventoryMenu.QueueFree();
	}

	public override Action GetAction(Player player)
	{
		Action action = null;

		if (ActivationItem != null)
		{
			action = new ItemAction(player, ActivationItem);
			Close();
		}

		if (DropItem != null)
		{
			action = new DropAction(player, DropItem);
			Close();
		}

		if (Input.IsActionJustPressed("quit"))
		{
			Close();
		}

		return action;
	}

	private void Close()
	{
		GetParent<InputHandler>().SetInputHandler(InputHandlers.MainGame);
	}

	private void OnItemActivate(ConsumableItem item)
	{
		ActivationItem = item;
	}

	private void OnItemDrop(ConsumableItem item)
	{
		DropItem = item;
	}
}