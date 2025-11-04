using System;
using Godot;
using TheLegendOfGustav.InputHandling;
using TheLegendOfGustav.Magic;

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

	[Signal]
	public delegate void PlayerSpellChooseLocationEventHandler(SpellResource spell);
	[Signal]
	public delegate void PlayerSpellCastEventHandler(bool success);

	[Signal]
	public delegate void CommandInputHandlerEventHandler(InputHandlers state);

	[Signal]
	public delegate void EscapeRequestedEventHandler();

	[Signal]
	public delegate void PlayerDescentEventHandler();

	[Signal]
	public delegate void DungeonFloorChangedEventHandler(int floor);

	public override void _Ready()
	{
		base._Ready();
		Instance = this;
	}
}
