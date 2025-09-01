using Godot;
using System;

/// <summary>
/// Objeto global com sinais, fortes sinais.
/// </summary>
public partial class SignalBus : Node
{
	/// <summary>
    /// Pois é.
    /// </summary>
	public static SignalBus Instance { get; private set; }

	public override void _Ready()
	{
		base._Ready();
		Instance = this;
	}
}
