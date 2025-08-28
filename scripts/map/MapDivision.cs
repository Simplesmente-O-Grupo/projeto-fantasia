using Godot;

/// <summary>
/// Classe utilizada pelo gerador de mapas.
/// Uma divisão é uma região retangular de espaço que pode
/// conter dentro de si duas regiões menores *ou* uma sala.
/// Uma divisão é uma árvore binária que possui espaço para salas em suas folhas.
/// </summary>
public partial class MapDivision : RefCounted {
	/// <summary>
    /// Região retangular da divisão.
    /// </summary>
	public Vector2I Position { get; set; }
	public Vector2I Size { get; set; }
 
	public Vector2I Center {
		get => new(Position.X + Size.X/2, Position.Y + Size.Y/2);
	}

	/// <summary>
    /// Filhos da árvore
    /// </summary>
	private MapDivision left;
	public MapDivision Left { get => this.left; }
	private MapDivision right;
	public MapDivision Right { get => this.right; }

	/// <summary>
    /// Se a divisão atual for uma folha.
    /// As folhas representam salas.
    /// </summary>
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

	/// <summary>
    /// É conveniente ter acesso à todas as folhas da árvore.
    /// </summary>
    /// <returns>Lista com todas as folhas da árvore.</returns>
	public Godot.Collections.Array<MapDivision> GetLeaves() {
		if (IsLeaf) {
			Godot.Collections.Array<MapDivision> list = [];
			list.Add(this);
			return list;
		}
		return left.GetLeaves() + right.GetLeaves();
	}

	/// <summary>
    /// Algoritmo para gerar as divisões.
    /// O mapa começa com uma única divisão que oculpa sua extensão completa.
    /// Depois disso, ela se dividirá recursivamente n vezes.
    /// As divisões nas folhas representam espaços onde pode gerar uma sala.
    /// </summary>
    /// <param name="iterations">Número de iterações</param>
    /// <param name="rng">Gerador de números</param>
	public void Split(int iterations, RandomNumberGenerator rng) {
		float SplitRatio = rng.RandfRange(0.35f, 0.65f);
		bool horizontalSplit = Size.X <= Size.Y;
		
		// Eu defini um limite mínimo de 4 de altura e 4 de largura para divisões.
		
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