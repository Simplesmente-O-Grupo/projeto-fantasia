using Godot;
using System;

public partial class Tile : Sprite2D
{
	private TileDefinition definition;

	public bool IsWalkable { get; private set; }
	public bool IsTransparent { get; private set; }

	private bool isExplored = false;
	public bool IsExplored {
		get => this.isExplored;
		set {
			isExplored = value;
			if (IsExplored && !Visible) {
				Visible = true;
			}
		}
	}

	private bool isInView = false;
	public bool IsInView {
		get => this.isInView;
		set {
			this.isInView = value;
			if (IsInView && !IsExplored) {
				IsExplored = true;
			}
		}
	}

	public Tile(Vector2I pos, TileDefinition definition)
	{
		Centered = false;
		Visible = false;
		Position = Grid.GridToWorld(pos);
		SetDefinition(definition);
	}

	public void SetDefinition(TileDefinition definition) {
		this.definition = definition;
		Texture = definition.Texture;
		IsWalkable = definition.IsWalkable;
		IsTransparent = definition.IsTransparent;
	}
}
