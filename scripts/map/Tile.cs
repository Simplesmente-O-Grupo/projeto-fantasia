using Godot;
using System;

public partial class Tile : Sprite2D
{
	private TileDefinition definition;

	public bool IsWalkable { get; private set; }

	public Tile(Vector2I pos, TileDefinition definition)
	{
		Centered = false;
		Position = Grid.GridToWorld(pos);
		SetDefinition(definition);
	}

	public void SetDefinition(TileDefinition definition) {
		this.definition = definition;
		Texture = definition.Texture;
		IsWalkable = definition.IsWalkable;
	}
}
