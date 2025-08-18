using Godot;
using System;

public partial class DungeonGenerator : Node
{
	private TileMapLayer map;

	[ExportCategory("Parametres")]
	[Export]
	private int width = 80;
	[Export]
	private int height = 60;

	public override void _Ready()
	{
		base._Ready();
		map = GetParent().GetNode<TileMapLayer>("Dungeon");
	}
}
