using Godot;

[GlobalClass]
public partial class EntityDefinition : Resource{
	[ExportCategory("Visuals")]
	// Nome da entidade.
	[Export]
	public string name = "unnamed";
	// Seu sprite.
	[Export]
	public Texture2D texture;

	[ExportCategory("Mechanics")]
	// Se a entidade bloqueia movimento.
	[Export]
	public bool blocksMovement = true;
}