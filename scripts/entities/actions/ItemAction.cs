using Godot;

public partial class ItemAction : Action
{
	private ConsumableItem item;
	public ItemAction(Actor actor, ConsumableItem item) : base(actor)
	{
		this.item = item;
	}

	public override bool Perform()
	{
		return item.Activate(this);
	}
}