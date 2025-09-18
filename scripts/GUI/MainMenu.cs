using Godot;

namespace TheLegendOfGustav.GUI;

public partial class MainMenu : Control
{
	private Button newGameButton;
	private Button loadGameButton;
	private Button quitButton;

	[Signal]
	public delegate void GameRequestEventHandler(bool load);

	public override void _Ready()
	{
		base._Ready();

		newGameButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/neogame");
		loadGameButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/continue");
		quitButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/quit");

		newGameButton.Pressed += OnNewGameButtonPressed;
		loadGameButton.Pressed += OnLoadGameButtonPressed;
		quitButton.Pressed += OnQuitButtonPressed;

		newGameButton.GrabFocus();
		bool hasSaveFile = FileAccess.FileExists("user://save.dat");
		loadGameButton.Disabled = !hasSaveFile;
	}

	private void OnNewGameButtonPressed()
	{
		GD.Print("Signal EMIT!");
		EmitSignal(SignalName.GameRequest, false);
	}

	private void OnLoadGameButtonPressed()
	{
		EmitSignal(SignalName.GameRequest, true);
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
