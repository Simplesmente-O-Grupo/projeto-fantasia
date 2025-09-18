using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities.Items;

namespace TheLegendOfGustav.Entities.Actions;

public partial class ItemAction : Action
{
	protected ConsumableItem item;

	public ItemAction(Player player, ConsumableItem item) : base(player)
	{
		this.item = item;
		Player = player;
	}

	public Player Player { get; private set; }

	public override bool Perform()
	{
		return item.Activate(this);
	}
}