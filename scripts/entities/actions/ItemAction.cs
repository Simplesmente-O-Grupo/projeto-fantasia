using Godot;

public partial class ItemAction : Action
{
	protected ConsumableItem item;
	protected Player player;
	public ItemAction(Player player, ConsumableItem item) : base(player)
	{
		this.item = item;
		this.player = player;
	}

	public override bool Perform()
	{
		return item.Activate(this);
	}
}