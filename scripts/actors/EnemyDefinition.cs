using Godot;

[GlobalClass]
public partial class EnemyDefinition : ActorDefinition {
	[ExportCategory("AI")]
	[Export]
	public AIType AI;
}