using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;
using TheLegendOfGustav.Entities.Items;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Actions;

public partial class PickupAction : DirectionalAction
{
	private Player player;

	public PickupAction(Player player, Vector2I offset) : base(player, offset)
	{
		this.player = player;
		// Pegar itens requer um tempo menor.
		cost = 2;
	}

	public override bool Perform()
	{
		ItemEntity item = MapData.GetFirstItemAtPosition(Destination);

		if (item == null)
		{
			MessageLogData.Instance.AddMessage("Não tem item aqui.");
			return false;
		}

		if (player.Inventory.Items.Count >= player.Inventory.Capacity)
		{
			MessageLogData.Instance.AddMessage("Seu inventário está cheio");
			return false;
		}

		MapData.RemoveEntity(item);
		player.Inventory.Add(item.Item);

		item.QueueFree();

		player.Energy -= cost;
		return true;
	}
}