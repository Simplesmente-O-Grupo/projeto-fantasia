using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Items;

public partial class HealingConsumable(Vector2I initialPosition, MapData map, HealingConsumableDefinition definition) : ConsumableItem(initialPosition, map, definition)
{
	private HealingConsumableDefinition Definition { get; set; } = definition;
	public float HealingPercentage { get; private set; } = definition.healingPercentage;

	public override bool Activate(ItemAction action)
	{
		Player consumer = action.Player;
		int intendedAmount = (int)(HealingPercentage / 100 * consumer.MaxHp);
		int recovered = consumer.Heal(intendedAmount);

		// Se não tinha o que curar, a ativação falhou.
		if (recovered == 0)
		{
			MessageLogData.Instance.AddMessage("Você já está saudável.");
			return false;
		}
		MessageLogData.Instance.AddMessage($"Você consome {DisplayName} e recupera {recovered} de HP");
		ConsumedBy(consumer);
		return true;
	}
}