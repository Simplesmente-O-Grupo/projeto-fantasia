using System.Runtime.InteropServices;
using Godot;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.GUI;

public partial class Hud : CanvasLayer
{
	private TextureProgressBar hpBar;
	private TextureProgressBar mpBar;
	private Label floorLabel;

	public override void _Ready()
	{
		base._Ready();
		hpBar = GetNode<TextureProgressBar>("VBoxContainer/InfoBar/Stats/MarginContainer/VBoxContainer/HBoxContainer/AspectRatioContainer/HPbar");
		mpBar = GetNode<TextureProgressBar>("VBoxContainer/InfoBar/Stats/MarginContainer/VBoxContainer/HBoxContainer/AspectRatioContainer/HPbar");
		floorLabel = GetNode<Label>("VBoxContainer/InfoBar/Stats/MarginContainer/VBoxContainer/HBoxContainer2/floorLabel");

		SignalBus.Instance.DungeonFloorChanged += OnFloorChanged;
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if (what == NotificationPredelete)
		{
			SignalBus.Instance.DungeonFloorChanged -= OnFloorChanged;
		}
	}

	public void OnHealthChanged(int hp, int maxHp)
	{
		hpBar.Value = hp;
		hpBar.MaxValue = maxHp;
	}

	public void OnManaChanged(int mp, int maxMp)
	{
		mpBar.Value = mp;
		mpBar.MaxValue = maxMp;
	}

	public void OnFloorChanged(int floor)
	{
		floorLabel.Text = $"Andar: {floor}";
	}
}
