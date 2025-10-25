using Godot;

namespace TheLegendOfGustav.Entities.Items;

/// <summary>
///  Esta classe só existe para agrupar seus descendentes.
/// </summary>
[GlobalClass]
public partial class ItemResource : Resource
{
	[Export]
	public string DisplayName { get; set; } = "Unnamed item";
	[Export]
	public Texture2D Icon { get; set; }
	[Export]
	public int MaxUses { get; set; } = 1;
	[Export]
	public string Description { get; set; } = "";

	[Export]
	public ItemActivation Activation { get; set; }
}