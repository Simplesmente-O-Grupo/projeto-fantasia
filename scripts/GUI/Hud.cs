using Godot;
using System;

public partial class Hud : Node
{
	private TextureProgressBar hpBar;

	public override void _Ready() {
		base._Ready();
		hpBar = GetNode<TextureProgressBar>("VBoxContainer/InfoBar/Stats/MarginContainer/HBoxContainer/AspectRatioContainer/HPbar");
	}

	public void OnHealthChanged(int hp, int maxHp) {
		hpBar.Value = hp;
		hpBar.MaxValue = maxHp;
	}
}
