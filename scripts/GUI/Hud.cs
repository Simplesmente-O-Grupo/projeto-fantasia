using Godot;
using System;

public partial class Hud : Node
{
	private TextureProgressBar hpBar;

	public override void _Ready() {
		base._Ready();
		hpBar = GetNode<TextureProgressBar>("InfoBar/Stats/MarginContainer/HBoxContainer/HPbar");
	}

	public void OnHealthChanged(int hp, int maxHp) {
		hpBar.Value = hp;
		hpBar.MaxValue = maxHp;
	}
}
