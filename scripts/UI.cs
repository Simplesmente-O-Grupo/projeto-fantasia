using Godot;
using System;

public partial class UI : Node2D
{
	private TextureProgressBar hpBar;

	public override void _Ready() {
		base._Ready();
		hpBar = GetNode<TextureProgressBar>("CanvasLayer/HPbar");
	}

	public void OnHealthChanged(int hp, int maxHp) {
		hpBar.Value = hp;
		hpBar.MaxValue = maxHp;
	}
}
