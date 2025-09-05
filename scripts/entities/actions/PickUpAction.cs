using Godot;

public partial class PickupAction : DirectionalAction
{
	protected Player player;

	public PickupAction(Player player, Vector2I offset) : base(player, offset)
	{
		this.player = player;
		// Pegar itens requer um tempo menor.
		cost = 2;
	}

	public override bool Perform()
	{
		ConsumableItem item = Map_Data.GetFirstItemAtPosition(Destination);

		if (item == null) {
			MessageLogData.Instance.AddMessage("Não tem item aqui.");
			return false;
		}

		if (player.inventory.Items.Count >= player.inventory.Capacity) {
			MessageLogData.Instance.AddMessage("Seu inventário está cheio");
			return false;
		}

		Map_Data.RemoveEntity(item);
		player.inventory.Add(item);

		player.Energy -= cost;
		return true;
	}
}