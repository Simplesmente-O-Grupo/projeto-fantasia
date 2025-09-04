using Godot;

/// <summary>
/// Isto é uma abominação
/// </summary>
public partial class Inspector : Sprite2D
{
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

	public override void _Ready() {
		base._Ready();
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		camera.Enabled = true;
		camera.MakeCurrent();

		SignalBus.Instance.EmitSignal(SignalBus.SignalName.InspectorMoved, GridPosition);
	}

	/// <summary>
    /// O Inspetor não faz parte do mapa.
    /// </summary>
    /// <param name="offset"></param>
	public void Walk(Vector2I offset) {
		GridPosition += offset;
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.InspectorMoved, GridPosition);
	}
}