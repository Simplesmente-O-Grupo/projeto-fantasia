using System.Linq;
using System.Numerics;
using System.Xml.Schema;
using Godot;

public partial class MapDivision : RefCounted {
	public Vector2I Position { get; set; }
	public Vector2I Size { get; set; }

	public Vector2I Center {
		get => new(Position.X + Size.X/2, Position.Y + Size.Y/2);
	}

	private MapDivision left;
	public MapDivision Left { get => this.left; }
	private MapDivision right;
	public MapDivision Right { get => this.right; }

	public bool IsLeaf {
		get => left == null && right == null;
	}

	public MapDivision(Vector2I position, Vector2I size) {
		Position = position;
		Size = size;
	}

	public MapDivision(Vector2I position, int width, int height) {
		Position = position;
		Size = new(width, height);
	}

	public MapDivision(int x, int y, int width, int height) {
		Position = new(x, y);
		Size = new(width, height);
	}

	public Godot.Collections.Array<MapDivision> GetLeaves() {
		if (IsLeaf) {
			Godot.Collections.Array<MapDivision> list = [];
			list.Add(this);
			return list;
		}
		return left.GetLeaves() + right.GetLeaves();
	}

	public void Split(int iterations, RandomNumberGenerator rng) {
		float SplitRatio = rng.RandfRange(0.35f, 0.65f);
		bool horizontalSplit = Size.X <= Size.Y;

		if (horizontalSplit) {
			int leftHeight = (int) (Size.Y * SplitRatio);
			if (leftHeight > 4 && Size.Y - leftHeight > 4) {
				left = new MapDivision(Position, Size.X, leftHeight);
				right = new MapDivision(Position.X, Position.Y + leftHeight, Size.X, Size.Y - leftHeight);
			}
		} else {
			int leftWidth = (int) (Size.Y * SplitRatio);

			if (leftWidth > 4 && Size.Y - leftWidth > 4) {
				left = new MapDivision(Position, leftWidth, Size.Y);
				right = new MapDivision(Position.X + leftWidth, Position.Y, Size.X - leftWidth, Size.Y);
			}
		}

		if (iterations > 1) {
			left?.Split(iterations - 1, rng);
			right?.Split(iterations - 1, rng);
		}
	}
}