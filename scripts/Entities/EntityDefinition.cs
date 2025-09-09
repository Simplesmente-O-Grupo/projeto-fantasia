using Godot;

namespace TheLegendOfGustav.Entities;

[GlobalClass]
public partial class EntityDefinition : Resource
{
	[ExportCategory("Entity Visuals")]
	// Nome da entidade.
	[Export]
	public string name = "unnamed";
	// Seu sprite.
	[Export]
	public Texture2D texture;
	// A camada da entidade.
	[Export]
	public EntityType Type;

	[ExportCategory("Entity Mechanics")]
	// Se a entidade bloqueia movimento.
	[Export]
	public bool blocksMovement = true;
}