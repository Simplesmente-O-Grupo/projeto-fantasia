using System.Diagnostics;
using Godot;
using TheLegendOfGustav.GUI;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav;

public partial class GameManager : Node
{
	private PackedScene mainMenuScene = GD.Load<PackedScene>("res://scenes/GUI/main_menu.tscn");
	private PackedScene gameScene = GD.Load<PackedScene>("res://scenes/Game.tscn");
	private PackedScene nameScene = GD.Load<PackedScene>("res://scenes/name_thyself.tscn");
	private PackedScene leaderboardScene = GD.Load<PackedScene>("res://scenes/GUI/Leaderboard.tscn");

	private Node currentScene;

	public override void _Ready()
	{
		base._Ready();
		LoadMainMenu();
	}

	private Node SwitchToScene(PackedScene scene)
	{
		if (currentScene != null && currentScene is Game gaimu)
		{
			gaimu.MainMenuRequested -= LoadMainMenu;
			gaimu.QueueFree();
			gaimu = null;
		} else if (currentScene != null && currentScene is MainMenu menu)
		{
			menu.QueueFree();
			menu.GameRequest -= OnGameRequest;
			menu = null;
		}

		currentScene?.QueueFree();
		currentScene = scene.Instantiate();
		AddChild(currentScene);
		return currentScene;
	}

	private void LoadMainMenu()
	{
		MainMenu menu = (MainMenu)SwitchToScene(mainMenuScene);
		Stats.Instance.Clear();
		menu.GameRequest += OnGameRequest;
		menu.LeaderboardRequest += OnLeaderboardRequest;
	}

	private void LoadGame()
	{
		MessageLogData.Instance.ClearMessages();
		Game game = (Game)SwitchToScene(gameScene);
		game.MainMenuRequested += LoadMainMenu;
		if (!game.LoadGame())
		{
			SwitchToScene(mainMenuScene);
		}
	}

	private void NewGame(string name)
	{
		MessageLogData.Instance.ClearMessages();
		Game game = (Game)SwitchToScene(gameScene);
		game.NewGame(name);
		game.MainMenuRequested += LoadMainMenu;
	}

	private void SelectName()
	{
		PlayerName namesc = (PlayerName)SwitchToScene(nameScene);
		namesc.NewGameRequest += OnNameSelect;
	}

	private void OnNameSelect(string name)
	{
		Stats.Instance.PlayerName = name;
		NewGame(name);
	}

	private void OnGameRequest(bool load)
	{
		if (!load)
		{
			SelectName();
		}
		else 
		{
			LoadGame();
		}
	}

	private void OnLeaderboardRequest()
	{
		Leaderboard scene = (Leaderboard)SwitchToScene(leaderboardScene);
		scene.MenuRequested += LoadMainMenu;
	}
}
