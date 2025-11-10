using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.Magic;

public enum SpellType
{
	Ranged,
	Self
}
[GlobalClass]
public partial class SpellResource : Resource
{
	/// <summary>
	/// Ícone do feitiço na interface gráfica.
	/// </summary>
	[ExportCategory("Visuals")]
	[Export]
	public Texture2D Icon { get; set; }
	/// <summary>
	/// Nome do feitiço na interface gráfica.
	/// </summary>
	[Export]
	public string SpellName { get; set; } = "unnamed spell";

	[ExportCategory("Mechanics")]
	[Export]
	public int Cost { get; set; }
	[Export]
	public SpellType Type { get; set; }
	[Export]
	public int Range { get; set; }
	[Export]
	public Godot.Collections.Array<SpellEffect> Effects { get; set; } = [];
}
