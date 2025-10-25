using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities.Items;

namespace TheLegendOfGustav.Entities.Actions;

public partial class DropAction : ItemAction
{
	public DropAction(Player player, Item item) : base(player, item)
	{
	}

	public override bool Perform()
	{
		Player.Inventory.Drop(item);
		return true;
	}
}