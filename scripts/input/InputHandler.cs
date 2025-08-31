using Godot;

public enum InputHandlers
{
	MainGame,
	GameOver
}

/// <summary>
/// Máquina de estado que obtém ações do usuário conforme o estado atual do jogo.
/// </summary>
public partial class InputHandler : Node
{
	private Godot.Collections.Dictionary<InputHandlers, BaseInputHandler> inputHandlers = [];

	[Export]
	private InputHandlers startingInputHandler;

	private BaseInputHandler selectedInputHandler;

	public override void _Ready()
	{
		base._Ready();
		// Controles para quando o jogador está vivo e jogando normalmente.
		inputHandlers.Add(InputHandlers.MainGame, GetNode<MainGameInputHandler>("MainGameInputHandler"));
		// Controles para quando o jogador está morto.
		inputHandlers.Add(InputHandlers.GameOver, GetNode<GameOverInputHandler>("GameOverInputHandler"));
		
		SetInputHandler(startingInputHandler);
	}
	
	public Action GetAction(Player player) {
		return selectedInputHandler.GetAction(player);
	}

	/// <summary>
    /// Define o esquema de controle atual do jogo
    /// para o estado informado.
    /// </summary>
    /// <param name="inputhandler">Estado do jogo.</param>
	public void SetInputHandler(InputHandlers inputhandler) {
		selectedInputHandler = inputHandlers[inputhandler];
	}
}
