using Godot;
using System;

public enum AIType
{
	None,
	DefaultHostile
};

public partial class Enemy : Actor
{
	public BaseAI Soul { get; private set; }

	public bool IsAlive { get => Soul != null; }

	public Enemy(Vector2I initialPosition, MapData map, EnemyDefinition definition) : base(initialPosition, map, definition)
	{
		SetDefinition(definition);
	}

	public void SetDefinition(EnemyDefinition definition)
	{
		base.SetDefinition(definition);

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
