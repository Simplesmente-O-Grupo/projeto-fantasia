using Godot;
using Godot.Collections;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Utils;

public partial class Stats : Node
{
	public static Stats Instance { get; set; }

	public string PlayerName { get; set; }
	public int MaxFloor { get; set; } = 0;

	public int EnemiesKilled { get; set; } = 0;

	public int DamageTaken { get; set; } = 0;

	public override void _Ready()
	{
		base._Ready();
		Instance = this;

		SignalBus.Instance.DungeonFloorChanged += OnFloorChange;
	}

	public void Clear()
	{
		PlayerName = "";
		MaxFloor = 0;
		EnemiesKilled = 0;
		DamageTaken = 0;
	}

	void OnFloorChange(int floor)
	{
		if (floor > MaxFloor)
		{
			MaxFloor = floor;
		}
	}

	public Dictionary<string, Variant> Serialize()
	{
		return new()
		{
			{"jogador", PlayerName},
			{"andar_mais_fundo", MaxFloor},
			{"inimigos_mortos", EnemiesKilled},
			{"dano_tomado", DamageTaken}
		};
	}
}