using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;
using TheLengendOfGustav.InputHandling;

namespace TheLegendOfGustav.InputHandling;

public enum InputHandlers
{
	MainGame,
	GameOver,
	Inspect,
	Pickup,
	Inventory,
	CastSpell,
	SpellMenu
}

/// <summary>
/// Máquina de estado que obtém ações do usuário conforme o estado atual do jogo.
/// </summary>
public partial class InputHandler : Node
{
	[Export]
	private InputHandlers StartingInputHandler { get; set; }

	private Godot.Collections.Dictionary<InputHandlers, BaseInputHandler> InputHandlerDict { get; set; } = [];

	private BaseInputHandler SelectedInputHandler { get; set; }

	public override void _Ready()
	{
		base._Ready();
		// Controles para quando o jogador está vivo e jogando normalmente.
		InputHandlerDict.Add(InputHandlers.MainGame, GetNode<MainGameInputHandler>("MainGameInputHandler"));
		// Controles para quando o jogador está morto.
		InputHandlerDict.Add(InputHandlers.GameOver, GetNode<GameOverInputHandler>("GameOverInputHandler"));
		// Controles para observar o cenário
		InputHandlerDict.Add(InputHandlers.Inspect, GetNode<InspectInputHandler>("InspectInputHandler"));
		// Controles para pegar um item do chão.
		InputHandlerDict.Add(InputHandlers.Pickup, GetNode<PickupInputHandler>("PickupInputHandler"));
		// Controles para quando o inventário for aberto.
		InputHandlerDict.Add(InputHandlers.Inventory, GetNode<InventoryInputHandler>("InventoryInputHandler"));
		// Controles para quando o jogador precisar escolher um alvo de feitiço.
		InputHandlerDict.Add(InputHandlers.CastSpell, GetNode<CastSpellInputHandler>("CastSpellInputHandler"));
		// Controles para quando o menu de feitiços for aberto.
		InputHandlerDict.Add(InputHandlers.SpellMenu, GetNode<SpellMenuInputHandler>("SpellMenuInputHandler"));

		SetInputHandler(StartingInputHandler);

		SignalBus.Instance.CommandInputHandler += SetInputHandler;
	}

	public Action GetAction(Player player)
	{
		return SelectedInputHandler.GetAction(player);
	}

	/// <summary>
	/// Define o esquema de controle atual do jogo
	/// para o estado informado.
	/// </summary>
	/// <param name="inputhandler">Estado do jogo.</param>
	public void SetInputHandler(InputHandlers inputhandler)
	{
		SelectedInputHandler?.Exit();
		SelectedInputHandler = InputHandlerDict[inputhandler];
		SelectedInputHandler.Enter();
	}
}
