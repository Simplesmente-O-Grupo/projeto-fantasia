using Godot;

public partial class InventoryInputHandler : BaseInputHandler
{
	private static readonly PackedScene inventoryScene = GD.Load<PackedScene>("res://scenes/GUI/invetory_menu.tscn");

	private InventoryMenu inventoryMenu;

	ConsumableItem activationItem = null;
	ConsumableItem dropItem = null;

	[Export]
	private Map map;

	public override void Enter() {
		inventoryMenu = inventoryScene.Instantiate<InventoryMenu>();
		map.Map_Data.Player.AddChild(inventoryMenu);
		inventoryMenu.Initialize(map.Map_Data.Player.inventory);
		inventoryMenu.ItemSelected += OnItemActivate;
		inventoryMenu.ItemDrop += OnItemDrop;
	}

	public override void Exit() {
		activationItem = null;
		dropItem = null;
		inventoryMenu.QueueFree();
	}

	public override Action GetAction(Player player)
	{
		Action action = null;

		if (activationItem != null) {
			action = new ItemAction(player, activationItem);
			Close();
		}

		if (dropItem != null) {
			action = new DropAction(player, dropItem);
			Close();
		}

		if (Input.IsActionJustPressed("quit")) {
			Close();
		}

		return action;
	}

	private void Close() {
		GetParent<InputHandler>().SetInputHandler(InputHandlers.MainGame);
	}

	private void ActivateItem() {

	}

	private void OnItemActivate(ConsumableItem item) {
		activationItem = item;
	}

	private void OnItemDrop(ConsumableItem item) {
		dropItem = item;
	}
}