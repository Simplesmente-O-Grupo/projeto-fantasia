using Godot;

// Copiado e adaptado deste cara aqui: https://www.roguebasin.com/index.php?title=C%2B%2B_shadowcasting_implementation e deste também https://selinadev.github.io/08-rogueliketutorial-04/

// Eu não vou mentir, li como o algoritmo funciona, mas mesmo assim não entendi.

public partial class FieldOfView : Node {

	private Godot.Collections.Array<Tile> fov = [];

	private static int[,] multipliers = new int[4,8]{
		{1, 0, 0, -1, -1, 0, 0, 1},
		{0, 1, -1, 0, 0, -1, 1, 0},
		{0, 1, 1, 0, 0, -1, -1, 0},
		{1, 0, 0, 1, -1, 0, 0, -1}
	};
	private void CastLight(MapData data, Vector2I pos, int radius, int row, float startSlope, float endSlope, int xx, int xy, int yx, int yy) {
		if (startSlope < endSlope) {
			return;
		}

		float nextStartSlope = startSlope;
		for (int i = row; i <= radius; i++)
		{
			bool blocked = false;
			for (int dx = -i, dy = -i; dx <= 0; dx++)
			{
				float lSlope = (float)((dx - 0.5) / (dy + 0.5));
				float rSlope = (float)((dx + 0.5) / (dy - 0.5));

				if (startSlope < rSlope)
				{
					continue;
				}
				else if (endSlope > lSlope)
				{
					break;
				}

				int sax = dx * xx + dy * xy;
				int say = dx * yx + dy * yy;

				if ((sax < 0 && int.Abs(sax) > pos.X) || (say < 0 && int.Abs(say) > pos.Y)) {
					continue;
				}
				int ax = pos.X + sax;
				int ay = pos.Y + say;

				if (ax >= data.Width || ay >= data.Height) {
					continue;
				}

				Tile currentTile = data.GetTile(ax, ay);
				int radius2 = radius * radius;
				if ((dx * dx + dy * dy) < radius2) {
					currentTile.IsInView = true;
					fov.Add(currentTile);
				}

				if (blocked) {
					if (!currentTile.IsTransparent) {
						nextStartSlope = rSlope;
						continue;
					} else {
						blocked = false;
						startSlope = nextStartSlope;
					}
				} else if (!currentTile.IsTransparent) {
					blocked = true;
					nextStartSlope = rSlope;
					CastLight(data, pos, radius, i + 1, startSlope, lSlope, xx, xy, yx, yy);
				}
			}
			if (blocked) {
				break;
			}
		}
	}

	private void ClearFOV() {
		foreach (Tile tile in fov) {
			tile.IsInView = false;
		}
		fov.Clear();
	}

	public void UpdateFOV(MapData data, Vector2I position, int radius) {
		ClearFOV();
		Tile start = data.GetTile(position);
		start.IsInView = true;
		fov.Add(start);
		for (int i = 0; i < 8; i++) {
			CastLight(data, position, radius, 1, 1.0f, 0.0f, multipliers[0, i], multipliers[1, i], multipliers[2, i], multipliers[3, i]);
		}
	}
}
