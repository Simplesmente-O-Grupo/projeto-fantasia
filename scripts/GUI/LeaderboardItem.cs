using Godot;

namespace TheLegendOfGustav.GUI;

public partial class LeaderboardItem : HBoxContainer
{
	[Export]
	public string PlayerName { get; set; } = "Jogador";
	[Export]
	public string Floor { get; set; } = "Andar Máximo";
	[Export]
	public string Kills { get; set; } = "Inimigos Mortos";
	[Export]
	public string Damage { get; set; } = "Dano tomado";

	private Label nameLabel;
	private Label floorLabel;
	private Label killsLabel;
	private Label damageLabel;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		nameLabel = GetNode<Label>("hdNome");
		floorLabel = GetNode<Label>("hdAndar");
		killsLabel = GetNode<Label>("hdkills");
		damageLabel = GetNode<Label>("hddamage");


		UpdateLabels();
	}

	public void UpdateLabels()
	{
		nameLabel.Text = PlayerName;
		floorLabel.Text = Floor;
		killsLabel.Text = Kills;
		damageLabel.Text = Damage;
	}
}
