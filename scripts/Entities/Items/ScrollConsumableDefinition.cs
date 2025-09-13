using Godot;
using TheLegendOfGustav.Magic;

namespace TheLegendOfGustav.Entities.Items;

[GlobalClass]
public partial class ScrollConsumableDefinition : ConsumableItemDefinition
{
	[Export]
	public SpellResource Spell { get; set; }
}