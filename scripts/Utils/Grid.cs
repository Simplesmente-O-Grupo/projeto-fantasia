using Godot;

namespace TheLegendOfGustav.Utils;

/// <summary>
/// Classe utilitária para converter coordenadas da malha dos tiles
/// em coordenadas em pixels.
/// Esta classe é necessária porque o Godot trata posições em pixels,
/// mas faz mais sentido tratarmos as posições em tiles.
/// </summary>
public abstract partial class Grid : GodotObject
{
	public static readonly Vector2I tileSize = new(16, 16);

	public static Vector2I WorldToGrid(Vector2 coord)
	{
		return (Vector2I)(coord / tileSize);
	}

	public static Vector2 GridToWorld(Vector2I coord)
	{
		return coord * tileSize;
	}
}
