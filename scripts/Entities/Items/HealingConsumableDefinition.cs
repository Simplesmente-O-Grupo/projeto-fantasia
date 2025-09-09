using Godot;

namespace TheLegendOfGustav.Entities.Items;

[GlobalClass]
public partial class HealingConsumableDefinition : ConsumableItemDefinition
{
	///<summary>
	/// Porcentagem da vida do ator para restaurar.
	///</summary>
	[ExportCategory("Item Mechanics")]
	[Export]
	public float healingPercentage = 10;
}