using Godot;

namespace TheLegendOfGustav.GUI;

public partial class MainMenu : Control
{
	private Button newGameButton;
	private Button loadGameButton;
	private Button quitButton;
	private Button leaderboardButton;

	[Signal]
	public delegate void GameRequestEventHandler(bool load);
	[Signal]
	public delegate void LeaderboardRequestEventHandler();

	public override void _Ready()
	{
		base._Ready();

		newGameButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/neogame");
		loadGameButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/continue");
		quitButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/quit");
		leaderboardButton = GetNode<Button>("VBoxContainer/CenterContainer/VBoxContainer/leaderboard");

		newGameButton.Pressed += OnNewGameButtonPressed;
		loadGameButton.Pressed += OnLoadGameButtonPressed;
		quitButton.Pressed += OnQuitButtonPressed;
		leaderboardButton.Pressed += OnLeaderBoardRequest;

		newGameButton.GrabFocus();
		bool hasSaveFile = FileAccess.FileExists("user://save_game.json");
		bool hasLeaderboard = FileAccess.FileExists("user://placar.json");
		loadGameButton.Disabled = !hasSaveFile;
		leaderboardButton.Disabled = !hasLeaderboard;
	}

	private void OnNewGameButtonPressed()
	{
		EmitSignal(SignalName.GameRequest, false);
	}

	private void OnLoadGameButtonPressed()
	{
		EmitSignal(SignalName.GameRequest, true);
	}

	private void OnLeaderBoardRequest()
	{
		EmitSignal(SignalName.LeaderboardRequest);
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
