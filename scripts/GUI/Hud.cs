using System.Runtime.InteropServices;
using Godot;

namespace TheLegendOfGustav.GUI;

public partial class Hud : Node
{
	private TextureProgressBar hpBar;
	private TextureProgressBar mpBar;

	public override void _Ready()
	{
		base._Ready();
		hpBar = GetNode<TextureProgressBar>("VBoxContainer/InfoBar/Stats/MarginContainer/HBoxContainer/AspectRatioContainer/HPbar");
		mpBar = GetNode<TextureProgressBar>("VBoxContainer/InfoBar/Stats/MarginContainer/HBoxContainer/AspectRatioContainer2/MPbar");
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
}
