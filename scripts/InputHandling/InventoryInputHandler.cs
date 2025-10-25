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
	private Map.Map map;

	private InventoryMenu inventoryMenu;
	private Item activationItem = null;
	private Item dropItem = null;

	public override void Enter()
	{
		inventoryMenu = inventoryScene.Instantiate<InventoryMenu>();
		map.MapData.Player.AddChild(inventoryMenu);
		inventoryMenu.Initialize(map.MapData.Player.Inventory);
		inventoryMenu.ItemSelected += OnItemActivate;
		inventoryMenu.ItemDrop += OnItemDrop;
	}

	public override void Exit()
	{
		activationItem = null;
		dropItem = null;
		inventoryMenu.QueueFree();
	}

	public override Action GetAction(Player player)
	{
		Action action = null;

		if (activationItem != null)
		{
			action = new ItemAction(player, activationItem);
			Close();
		}

		if (dropItem != null)
		{
			action = new DropAction(player, dropItem);
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

	private void OnItemActivate(Item item)
	{
		activationItem = item;
	}

	private void OnItemDrop(Item item)
	{
		dropItem = item;
	}
}