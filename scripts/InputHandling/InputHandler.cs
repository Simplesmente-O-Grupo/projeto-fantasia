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
	private InputHandlers startingInputHandler;

	private Godot.Collections.Dictionary<InputHandlers, BaseInputHandler> inputHandlerDict = [];

	private BaseInputHandler selectedInputHandler;

	public override void _Ready()
	{
		base._Ready();
		// Controles para quando o jogador está vivo e jogando normalmente.
		inputHandlerDict.Add(InputHandlers.MainGame, GetNode<MainGameInputHandler>("MainGameInputHandler"));
		// Controles para quando o jogador está morto.
		inputHandlerDict.Add(InputHandlers.GameOver, GetNode<GameOverInputHandler>("GameOverInputHandler"));
		// Controles para observar o cenário
		inputHandlerDict.Add(InputHandlers.Inspect, GetNode<InspectInputHandler>("InspectInputHandler"));
		// Controles para pegar um item do chão.
		inputHandlerDict.Add(InputHandlers.Pickup, GetNode<PickupInputHandler>("PickupInputHandler"));
		// Controles para quando o inventário for aberto.
		inputHandlerDict.Add(InputHandlers.Inventory, GetNode<InventoryInputHandler>("InventoryInputHandler"));
		// Controles para quando o jogador precisar escolher um alvo de feitiço.
		inputHandlerDict.Add(InputHandlers.CastSpell, GetNode<CastSpellInputHandler>("CastSpellInputHandler"));
		// Controles para quando o menu de feitiços for aberto.
		inputHandlerDict.Add(InputHandlers.SpellMenu, GetNode<SpellMenuInputHandler>("SpellMenuInputHandler"));

		SetInputHandler(startingInputHandler);

		SignalBus.Instance.CommandInputHandler += SetInputHandler;
	}

	public Action GetAction(Player player)
	{
		return selectedInputHandler.GetAction(player);
	}

	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
		{
			SignalBus.Instance.CommandInputHandler -= SetInputHandler;
		}
		base._Notification(what);
	}

	/// <summary>
	/// Define o esquema de controle atual do jogo
	/// para o estado informado.
	/// </summary>
	/// <param name="inputhandler">Estado do jogo.</param>
	public void SetInputHandler(InputHandlers inputhandler)
	{
		selectedInputHandler?.Exit();
		selectedInputHandler = inputHandlerDict[inputhandler];
		selectedInputHandler.Enter();
	}
}
