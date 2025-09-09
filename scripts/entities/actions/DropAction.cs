using Godot;

public partial class DropAction : ItemAction
{
	public DropAction(Player player, ConsumableItem item) : base(player, item)
	{
	}

	public override bool Perform() {
		player.inventory.Drop(item);
		return true;
	}
}