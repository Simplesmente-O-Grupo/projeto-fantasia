using Godot;

namespace TheLegendOfGustav.GUI;

public partial class MainMenu : Control
{
	private Button NewGameButton;
	private Button LoadGameButton;
	private Button QuitButton;

	[Signal]
	public delegate void GameRequestEventHandler(bool load);

	public override void _Ready()
	{
		base._Ready();

		NewGameButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/neogame");
		LoadGameButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/continue");
		QuitButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/quit");

		NewGameButton.Pressed += OnNewGameButtonPressed;
		LoadGameButton.Pressed += OnLoadGameButtonPressed;
		QuitButton.Pressed += OnQuitButtonPressed;

		NewGameButton.GrabFocus();
		bool hasSaveFile = FileAccess.FileExists("user://save.dat");
		LoadGameButton.Disabled = !hasSaveFile;
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
