using Godot;
using System;

/// <summary>
/// Enum das diferentes IAs disponíveis.
/// </summary>
public enum AIType
{
	None,
	DefaultHostile
};

/// <summary>
/// Um inimigo é uma espécie de ator que é
/// hostil ao jogador. Inimigos são controlados por IA.
/// </summary>
public partial class Enemy : Actor
{
	/// <summary>
    /// A alma do ator. Gera ações que são executadas todo turno.
    /// </summary>
	public BaseAI Soul { get; private set; }

	public Enemy(Vector2I initialPosition, MapData map, EnemyDefinition definition) : base(initialPosition, map, definition)
	{
		SetDefinition(definition);
	}

	/// <summary>
    /// Além de definir as características gerais de um ator,
    /// também define qual IA utilizar.
    /// </summary>
    /// <param name="definition">Definição do inimigo.</param>
	public void SetDefinition(EnemyDefinition definition)
	{
		// Definimos as características do ator.
		base.SetDefinition(definition);

		// Definimos qual IA utilizar.
		switch(definition.AI) {
			case AIType.None:
				break;
			case AIType.DefaultHostile:
				Soul = new HostileEnemyAI();
				AddChild(Soul);
				break;
		}
	}
}
