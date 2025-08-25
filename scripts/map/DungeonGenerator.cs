using Godot;
using System;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;

public partial class DungeonGenerator : Node
{
	[ExportCategory("Dimension")]
	[Export]
	private int width = 80;
	[Export]
	private int height = 60;

	[ExportCategory("RNG")]
	private RandomNumberGenerator rng = new();
	[Export]
	private ulong seed;
	[Export]
	private bool useSeed = true;
	[Export]
	private int iterations = 3;

	public override void _Ready()
	{
		base._Ready();
		if (useSeed) {
			rng.Seed = seed;
		}
	}

	private static void CarveTile(MapData data, Vector2I pos)
	{
		Tile tile = data.GetTile(pos);
		if (tile == null) return;

		tile.SetDefinition(MapData.floorDefinition);
	}

	private static void CarveRoom(MapData data, Rect2I room)
	{
		for (int y = room.Position.Y; y < room.End.Y; y++)
		{
			for (int x = room.Position.X; x < room.End.X; x++)
			{
				CarveTile(data, new Vector2I(x, y));
			}
		}
	}

	public MapData GenerateDungeon(Player player)
	{
		MapData data = new MapData(width, height, player);

		data.InsertActor(player);
		player.Map_Data = data;

		MapDivision root = new MapDivision(0, 0, width, height);

		root.Split(iterations, rng);

		bool first = true;

		TunnelDivisions(data, root);

		foreach(MapDivision division in root.GetLeaves())
		{
			Rect2I room = new(division.Position, division.Size);
			
			room = room.GrowIndividual(
				-rng.RandiRange(1, 2),
				-rng.RandiRange(1, 2),
				-rng.RandiRange(1, 2),
				-rng.RandiRange(1, 2)
			);

			GD.Print($"Division {room}");
			CarveRoom(data, room);
			if (first)
			{
				first = false;
				player.GridPosition = room.GetCenter();
			}
		}

		return data;
	}

	private static void HorizontalCorridor(MapData data, int y, int xBegin, int xEnd) {
		int begin = (xBegin < xEnd) ? xBegin : xEnd;
		int end = (xEnd > xBegin) ? xEnd : xBegin;
		for (int i = begin; i <= end; i++) {
			CarveTile(data, new Vector2I(i, y));
		}
	}

	private static void VerticalCorridor(MapData data, int x, int yBegin, int yEnd) {
		int begin = (yBegin < yEnd) ? yBegin : yEnd;
		int end = (yEnd > yBegin) ? yEnd : yBegin;
		for (int i = begin; i <= end; i++) {
			CarveTile(data, new Vector2I(x, i));
		}
	}

	private void TunnelBetween(MapData data, Vector2I start, Vector2I end) {
		HorizontalCorridor(data, start.Y, start.X, end.X);
		VerticalCorridor(data, end.X, start.Y, end.Y);
	}

	private void TunnelDivisions(MapData data, MapDivision root) {
		if (root.IsLeaf) {
			return;
		}

		TunnelBetween(data, root.Right.Center, root.Left.Center);
		TunnelDivisions(data, root.Left);
		TunnelDivisions(data, root.Right);
	}
}
