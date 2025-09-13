using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Magic;
using TheLegendOfGustav.Map;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Items;

public partial class ScrollConsumable(Vector2I initialPosition, MapData map, ScrollConsumableDefinition definition) : ConsumableItem(initialPosition, map, definition)
{
	private ScrollConsumableDefinition definition = definition;

	public SpellResource Spell { get; private set; } = definition.Spell;


	public override bool Activate(ItemAction action)
	{
		Player consumer = action.Player;

		MessageLogData.Instance.AddMessage("Foste cuckado");
		ConsumedBy(consumer);
		return true;
	}
}