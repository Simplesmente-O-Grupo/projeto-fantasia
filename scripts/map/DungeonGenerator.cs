using Godot;
using System;
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
	private int maxRooms = 10;
	[Export]
	private int roomMaxWidth = 10;
	[Export]
	private int roomMinWidth = 3;
	[Export]
	private int roomMaxHeight = 10;
	[Export]
	private int roomMinHeight = 2;

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
		Rect2I inner = room.Grow(-1);

		for (int y = inner.Position.Y; y <= inner.End.Y; y++)
		{
			for (int x = inner.Position.X; x <= inner.End.X; x++)
			{
				CarveTile(data, new Vector2I(x, y));
			}
		}
	}

	public MapData GenerateDungeon(Player player)
	{
		MapData data = new MapData(width, height);

		Godot.Collections.Array<Rect2I> rooms = [];

		for (int tryroom = 0; tryroom < maxRooms; tryroom++) {
			int roomWidth = rng.RandiRange(roomMinWidth, roomMaxWidth);
			int roomHeight = rng.RandiRange(roomMinHeight, roomMaxHeight);

			int x = rng.RandiRange(0, data.Width - 1 - roomWidth);
			int y = rng.RandiRange(0, data.Height - 1 - roomHeight);

			Rect2I newRoom = new(x, y, roomWidth, roomHeight);

			bool intersects = false;
			foreach (Rect2I room in rooms) {
				if (newRoom.Intersects(room)) {
					intersects = true;
					break;
				}
			}
			if (intersects) {
				continue;
			}

			CarveRoom(data, newRoom);
			if (rooms.Count <= 0) {
				player.GridPosition = newRoom.GetCenter();
			} else {
				TunnelBetween(data, rooms.Last().GetCenter(), newRoom.GetCenter());
			}
			rooms.Add(newRoom);
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
}
