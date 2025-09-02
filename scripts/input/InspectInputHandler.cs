using Godot;

/// <summary>
/// TODO: Esta solução é nojenta e precisa ser retrabalhada.
/// </summary>
public partial class InspectInputHandler : BaseInputHandler
{
	private readonly Godot.Collections.Dictionary<string, Vector2I> directions = new()
	{
		{"walk-up", Vector2I.Up},
		{"walk-down", Vector2I.Down},
		{"walk-left", Vector2I.Left},
		{"walk-right", Vector2I.Right},
		{"walk-up-right", Vector2I.Up + Vector2I.Right},
		{"walk-up-left", Vector2I.Up + Vector2I.Left},
		{"walk-down-right", Vector2I.Down + Vector2I.Right},
		{"walk-down-left", Vector2I.Down + Vector2I.Left},
	};
	/// <summary>
    /// Preciso disso
    /// </summary>
	[Export]
	private Map map;

	private Inspector inspector;

	public override void Enter() {
		inspector = new(map.Map_Data.Player.GridPosition)
		{
			ZIndex = 4
		};
		// Copiamos a câmera do jogador com todas as suas configurações
		Camera2D camera = (Camera2D) map.Map_Data.Player.GetNode<Camera2D>("Camera2D").Duplicate();
		inspector.AddChild(camera);

		map.AddChild(inspector);
		camera.Enabled = true;
		camera.MakeCurrent();
	}

	public override void Exit() {
		inspector.QueueFree();
	}
	public override Action GetAction(Player player)
	{
		Action action = null;
		foreach (var direction in directions) {
			if (Input.IsActionJustPressed(direction.Key)) {
				inspector.Walk(direction.Value);
			}
		}

		if (Input.IsActionJustPressed("quit")) {
			GetParent<InputHandler>().SetInputHandler(InputHandlers.MainGame);
		}

		return action;
	}
}