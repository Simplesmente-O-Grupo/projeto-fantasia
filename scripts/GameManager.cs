using Godot;
using GodotPlugins.Game;
using System;
using TheLegendOfGustav.GUI;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav;

public partial class GameManager : Node
{
	private PackedScene mainMenuScene = GD.Load<PackedScene>("res://scenes/GUI/main_menu.tscn");
	private PackedScene gameScene = GD.Load<PackedScene>("res://scenes/Game.tscn");

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
		menu.GameRequest += OnGameRequest;
	}

	private void LoadGame()
	{
		MessageLogData.Instance.ClearMessages();
		Game game = (Game)SwitchToScene(gameScene);
		game.MainMenuRequested += LoadMainMenu;
	}

	private void OnGameRequest(bool load)
	{
		LoadGame();
	}
}
