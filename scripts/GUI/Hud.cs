using Godot;

namespace TheLegendOfGustav.GUI;

public partial class Hud : Node
{
	private TextureProgressBar HpBar { get; set; }

	public override void _Ready()
	{
		base._Ready();
		HpBar = GetNode<TextureProgressBar>("VBoxContainer/InfoBar/Stats/MarginContainer/HBoxContainer/AspectRatioContainer/HPbar");
	}

	public void OnHealthChanged(int hp, int maxHp)
	{
		HpBar.Value = hp;
		HpBar.MaxValue = maxHp;
	}
}
