using Godot;
using System;

[GlobalClass]
public partial class ActorDefinition : Resource
{
	[ExportCategory("Visuals")]
	[Export]
	public string name = "unnamed";
	[Export]
	public Texture2D texture;

	[ExportCategory("Mechanics")]
	[Export]
	public bool blocksMovement = true;

	[ExportCategory("Stats")]
	[Export]
	public int Hp;
	[Export]
	public int Mp;
	[Export]
	public int Atk;
	[Export]
	public int Def;
	[Export]
	public int Men;
}
