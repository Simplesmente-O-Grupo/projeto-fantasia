using Godot;
using TheLegendOfGustav.Entities.Actors.AI;

namespace TheLegendOfGustav.Entities.Actors;

/// <summary>
/// Além das configurações do ator, também possui qual IA utilizar.
/// </summary>
[GlobalClass]
public partial class EnemyDefinition : ActorDefinition
{
	[ExportCategory("AI")]
	[Export]
	public AIType AI;
}