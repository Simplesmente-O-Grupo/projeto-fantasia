using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Magic;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Items;

public partial class GrimoireConsumable(Vector2I initialPosition, MapData map, GrimoireConsumableDefinition definition) : ConsumableItem(initialPosition, map, definition)
{
	private SpellResource spell = definition.Spell;

	public override bool Activate(ItemAction action)
	{
		Player consumer = action.Player;

		consumer.SpellBook.LearnSpell(spell);

		ConsumedBy(consumer);
		return true;
	}
}