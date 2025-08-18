using Godot;
using System;

[GlobalClass]
public partial class TileDefinition : Resource
{
	[ExportCategory("Visuals")]
	[Export]
	public Texture2D Texture { get; set; }

	[ExportCategory("Mechanics")]
	[Export]
	public bool IsWalkable { get; set; }
}
