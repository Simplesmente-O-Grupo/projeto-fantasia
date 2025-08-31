using Godot;
using System;

public partial class UI : Node
{
	private TextureProgressBar hpBar;

	public override void _Ready() {
		base._Ready();
		hpBar = GetNode<TextureProgressBar>("CanvasLayer/MainContainer/HPbar");
	}

	public void OnHealthChanged(int hp, int maxHp) {
		hpBar.Value = hp;
		hpBar.MaxValue = maxHp;
	}
}
