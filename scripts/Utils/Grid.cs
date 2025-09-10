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
	/// <summary>
    /// Tamanho de cada tile em pixels.
    /// </summary>
	public static readonly Vector2I tileSize = new(16, 16);

	/// <summary>
    /// Converte coordenadas do mundo (em pixels) para coordenadas do mapa (tiles).
    /// </summary>
    /// <param name="coord">Coordenada em pixels.</param>
    /// <returns>Coordenada em tiles.</returns>
	public static Vector2I WorldToGrid(Vector2 coord)
	{
		return (Vector2I)(coord / tileSize);
	}

	/// <summary>
    /// Converte coordenadas do mapa (em tiles) para coordenadas do mundo (em pixels)
    /// </summary>
    /// <param name="coord">Coordenada em tiles</param>
    /// <returns>Coordenada em pixels.</returns>
	public static Vector2 GridToWorld(Vector2I coord)
	{
		return coord * tileSize;
	}

	/// <summary>
    /// Calcula a distância entre pontos A e B no mapa.
    /// </summary>
    /// <remarks>
    /// A distância retornada não é a distância real mas sim a
    /// maior distância em um único eixo.
    /// Isso significa que uma área em volta de um ponto não é um
    /// círculo, mas um quadrado.
    /// Veja: https://en.wikipedia.org/wiki/Chebyshev_distance
    /// </remarks>
    /// <param name="a">Um ponto no mapa</param>
    /// <param name="b">Um ponto no mapa</param>
    /// <returns>A distância entre pontos <c>a</c> e <c>b.</c></returns>
	public static int Distance(Vector2I a, Vector2I b) {
		Vector2I distanceVector = b - a;
		return int.Max(int.Abs(distanceVector.X), int.Abs(distanceVector.Y));
	}
}
