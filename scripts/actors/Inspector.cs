using Godot;

/// <summary>
/// Isto é uma abominação
/// </summary>
public partial class Inspector : Sprite2D
{
	private static readonly Texture2D texture = GD.Load<Texture2D>("res://assets/sprites/inspector.png");

	private Vector2I gridPosition = Vector2I.Zero;
	/// <summary>
    /// Posição do inspetor no espaço. Diferentemente de Position, GridPosition tem como formato 
    /// os tiles do mapa.
    /// </summary>
	public Vector2I GridPosition {
		set {
			gridPosition = value;
			// O sistema de coordenadas do Godot é em pixels, mas faz mais sentido para o jogo utilizar coordenadas em tiles.
			// Esta propriedade converte um sistema para o outro automaticamente.
			Position = Grid.GridToWorld(value);
		}
		get => gridPosition;
	}

	public Inspector(Vector2I initialPosition)
	{
		GridPosition = initialPosition;
		Centered = false;
		Texture = texture;
	}

	/// <summary>
    /// O Inspetor não faz parte do mapa.
    /// </summary>
    /// <param name="offset"></param>
	public void Walk(Vector2I offset) {
		GridPosition += offset;
	}
}