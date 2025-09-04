using Godot;

public partial class HealingConsumable : ConsumableItem
{
	private HealingConsumableDefinition definition;
	public float HealingPercentage { get; private set; }
	public HealingConsumable(Vector2I initialPosition, MapData map, HealingConsumableDefinition definition) : base(initialPosition, map, definition)
	{
		this.definition = definition;
		HealingPercentage = definition.healingPercentage;
	}

	public override bool Activate(ItemAction action)
	{
		Actor consumer = action.ThisActor;
		int intendedAmount = (int)(HealingPercentage / 100 * consumer.MaxHp);
		int recovered = consumer.Heal(intendedAmount);

		// Se não tinha o que curar, a ativação falhou.
		if (recovered == 0) {
			MessageLogData.Instance.AddMessage("Você já está saudável.");
			return false;
		}
		MessageLogData.Instance.AddMessage($"Você consome {DisplayName} e recupera {recovered} de HP");
		return true;
	}
}