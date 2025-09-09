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
		Player = player;
		// Pegar itens requer um tempo menor.
		Cost = 2;
	}

	protected Player Player
	{
		get => player;
		private set
		{
			player = value;
		}
	}

	public override bool Perform()
	{
		ConsumableItem item = MapData.GetFirstItemAtPosition(Destination);

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
		player.Inventory.Add(item);

		player.Energy -= Cost;
		return true;
	}
}