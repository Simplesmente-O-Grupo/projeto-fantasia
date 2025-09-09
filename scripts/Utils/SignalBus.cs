using Godot;

namespace TheLegendOfGustav.Utils;

/// <summary>
/// Objeto global com sinais, fortes sinais.
/// </summary>
public partial class SignalBus : Node
{
	/// <summary>
	/// Pois é.
	/// </summary>
	public static SignalBus Instance { get; private set; }

	[Signal]
	public delegate void InspectorMovedEventHandler(Vector2I pos);

	[Signal]
	public delegate void EnterInspectionModeEventHandler();
	[Signal]
	public delegate void ExitInspectionModeEventHandler();

	public override void _Ready()
	{
		base._Ready();
		Instance = this;
	}
}
