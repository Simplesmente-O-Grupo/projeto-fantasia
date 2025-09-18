using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.InputHandling;

/// <summary>
/// TODO: Esta solução é nojenta e precisa ser retrabalhada.
/// </summary>
public partial class InspectInputHandler : BaseInputHandler
{
	private static readonly PackedScene inspectorScene = GD.Load<PackedScene>("res://scenes/Inspector.tscn");

	private static readonly Godot.Collections.Dictionary<string, Vector2I> directions = new()
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
	private Map.Map map;

	private Inspector inspector;

	public override void Enter()
	{
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnterInspectionMode);
		inspector = inspectorScene.Instantiate<Inspector>();

		inspector.GridPosition = map.MapData.Player.GridPosition;

		map.AddChild(inspector);
	}

	public override void Exit()
	{
		inspector.QueueFree();

		SignalBus.Instance.EmitSignal(SignalBus.SignalName.ExitInspectionMode);
	}

	public override Action GetAction(Player player)
	{
		Action action = null;
		foreach (var direction in directions)
		{
			if (Input.IsActionJustPressed(direction.Key))
			{
				inspector.Walk(direction.Value);
			}
		}

		if (Input.IsActionJustPressed("quit"))
		{
			GetParent<InputHandler>().SetInputHandler(InputHandlers.MainGame);
		}

		return action;
	}
}