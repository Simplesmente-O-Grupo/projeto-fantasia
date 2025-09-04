using Godot;

/// <summary>
/// Define de forma genérica as características de um ator.
/// </summary>
[GlobalClass]
public partial class ActorDefinition : EntityDefinition
{
	[ExportCategory("Visuals")]
	// Sprite de morto
	[Export]
	public Texture2D deathTexture;

	[ExportCategory("Mechanics")]
	[Export]
	public int Speed { get; set;} = 10;

	// Estatísticas padrão do ator.
	[ExportCategory("Stats")]
	[Export]
	public int Hp;
	[Export]
	public int Mp;
	[Export]
	public int Atk;
	[Export]
	public int Def;
	[Export]
	public int Men;
}
