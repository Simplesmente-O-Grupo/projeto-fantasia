using Godot;
using Godot.Collections;
using TheLegendOfGustav.Entities.Actors.AI;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Actors;

/// <summary>
/// Um inimigo é uma espécie de ator que é
/// hostil ao jogador. Inimigos são controlados por IA.
/// </summary>
public partial class Enemy : Actor, ISaveable
{
	private EnemyDefinition definition;

	public Enemy(Vector2I initialPosition, MapData map, EnemyDefinition definition) : base(initialPosition, map, definition)
	{
		this.definition = definition;
		SetDefinition(definition);
	}
	public Enemy(Vector2I initialPosition, MapData map) : base(initialPosition, map)
	{
	}

	/// <summary>
	/// A alma do ator. Gera ações que são executadas todo turno.
	/// </summary>
	public BaseAI Soul { get; private set; }

	/// <summary>
	/// Além de definir as características gerais de um ator,
	/// também define qual IA utilizar.
	/// </summary>
	/// <param name="definition">Definição do inimigo.</param>
	public void SetDefinition(EnemyDefinition definition)
	{
		this.definition = definition;
		// Definimos as características do ator.
		base.SetDefinition(this.definition);

		Soul?.QueueFree();

		// Definimos qual IA utilizar.
		switch (definition.AI)
		{
			case AIType.None:
				break;
			case AIType.DefaultHostile:
				Soul = new HostileEnemyAI();
				AddChild(Soul);
				break;
		}
	}

	public override void Die()
	{
		Soul.QueueFree();
		Soul = null;
		base.Die();
	}

	public new Dictionary<string, Variant> GetSaveData()
	{
		Dictionary<string, Variant> baseData = base.GetSaveData();
		baseData.Add("definition", definition.ResourcePath);

		return baseData;
	}

	public new bool LoadSaveData(Dictionary<string, Variant> saveData)
	{
		string definitionPath = (string)saveData["definition"];
		EnemyDefinition definition = GD.Load<EnemyDefinition>(definitionPath);

		SetDefinition(definition);

		if (!base.LoadSaveData(saveData))
		{
			return false;
		}

		return true;
	}
}
