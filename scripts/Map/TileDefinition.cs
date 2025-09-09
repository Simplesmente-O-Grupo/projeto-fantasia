using Godot;

namespace TheLegendOfGustav.Map;

/// <summary>
/// Define as características de um tile.
/// </summary>
[GlobalClass]
public partial class TileDefinition : Resource
{
	[ExportCategory("Visuals")]
	[Export]
	public Texture2D Texture { get; set; }
	[Export(PropertyHint.ColorNoAlpha)]
	public Color LitColor { get; set; } = Colors.White;
	[Export(PropertyHint.ColorNoAlpha)]
	public Color DarkColor { get; set; } = Colors.White;

	[ExportCategory("Mechanics")]
	[Export]
	public bool IsWalkable { get; set; }
	[Export]
	public bool IsTransparent { get; set; }
}
