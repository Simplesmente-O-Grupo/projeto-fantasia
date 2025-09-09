using Godot;
using TheLegendOfGustav.Entities.Actors.AI;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Actors;

/// <summary>
/// Um inimigo é uma espécie de ator que é
/// hostil ao jogador. Inimigos são controlados por IA.
/// </summary>
public partial class Enemy : Actor
{
	public Enemy(Vector2I initialPosition, MapData map, EnemyDefinition definition) : base(initialPosition, map, definition)
	{
		Definition = definition;
		SetDefinition(definition);
	}

	/// <summary>
	/// A alma do ator. Gera ações que são executadas todo turno.
	/// </summary>
	public BaseAI Soul { get; private set; }

	private EnemyDefinition Definition { get; set; }

	/// <summary>
	/// Além de definir as características gerais de um ator,
	/// também define qual IA utilizar.
	/// </summary>
	/// <param name="definition">Definição do inimigo.</param>
	public void SetDefinition(EnemyDefinition definition)
	{
		// Definimos as características do ator.
		base.SetDefinition(Definition);

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
}
