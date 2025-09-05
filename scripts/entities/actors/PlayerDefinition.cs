using Godot;

[GlobalClass]
public partial class PlayerDefinition : ActorDefinition {
	[ExportCategory("Player Mechanics")]
	[Export]
	public int InventoryCapacity = 0;
}