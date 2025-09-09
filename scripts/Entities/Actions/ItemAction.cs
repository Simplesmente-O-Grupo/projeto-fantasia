using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities.Items;

namespace TheLegendOfGustav.Entities.Actions;

public partial class ItemAction : Action
{
	private ConsumableItem item;
	private Player player;

	public ItemAction(Player player, ConsumableItem item) : base(player)
	{
		Item = item;
		Player = player;
	}

	public Player Player
	{
		get => player;
		private set
		{
			player = value;
		}
	}

	protected ConsumableItem Item
	{
		get => item;
		set
		{
			item = value;
		}
	}

	public override bool Perform()
	{
		return item.Activate(this);
	}
}