using Godot;

/// <summary>
/// TODO: Esta solução é nojenta e precisa ser retrabalhada.
/// </summary>
public partial class InspectInputHandler : BaseInputHandler
{
	private static readonly PackedScene InspectorScene = GD.Load<PackedScene>("res://scenes/Inspector.tscn");

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
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnterInspectionMode);
		inspector = InspectorScene.Instantiate<Inspector>();

		inspector.GridPosition = map.Map_Data.Player.GridPosition;

		map.AddChild(inspector);
	}

	public override void Exit() {
		inspector.QueueFree();

		SignalBus.Instance.EmitSignal(SignalBus.SignalName.ExitInspectionMode);
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