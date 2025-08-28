using Godot;

/// <summary>
/// Define de forma genérica as características de um ator.
/// </summary>
[GlobalClass]
public partial class ActorDefinition : Resource
{
	[ExportCategory("Visuals")]
	// Nome do ator.
	[Export]
	public string name = "unnamed";
	// Seu sprite.
	[Export]
	public Texture2D texture;
	// Sprite de morto
	[Export]
	public Texture2D deathTexture;

	[ExportCategory("Mechanics")]
	// Se o ator bloqueia movimento.
	[Export]
	public bool blocksMovement = true;

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
