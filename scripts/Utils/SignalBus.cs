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
	/// <summary>
	/// Acionado sempre que uma mensagem for adicionada para o log.
	/// </summary>
	/// <param name="text">Mensagem.</param>
	[Signal]
	public delegate void messageSentEventHandler(string text);
}
