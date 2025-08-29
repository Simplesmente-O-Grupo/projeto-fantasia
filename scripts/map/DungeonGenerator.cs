using Godot;

/// <summary>
/// A classe dungeonGenerator cria exatamente um andar da masmorra.
/// Ela é chamada quando necessário.
/// </summary>
public partial class DungeonGenerator : Node
{
	/// <summary>
    /// Coleção de todos os inimigos que o gerador tem acesso.
    /// </summary>
	private static readonly Godot.Collections.Array<EnemyDefinition> enemies = [
		GD.Load<EnemyDefinition>("res://assets/definitions/actor/Skeleton.tres"),
		GD.Load<EnemyDefinition>("res://assets/definitions/actor/morcegao.tres")
	];

	/// <summary>
    /// Dimensões do mapa a ser criado.
    /// </summary>
	[ExportCategory("Dimension")]
	[Export]
	private int width = 80;
	[Export]
	private int height = 60;

	/// <summary>
	/// Gerador de números aleatórios
	/// </summary>
	[ExportCategory("RNG")]
	private RandomNumberGenerator rng = new();
	/// <summary>
    /// Qual seed utilizar.
    /// </summary>
	[Export]
	private ulong seed;
	/// <summary>
    /// Se será utilizada a nossa seed ou a seed padrão da classe RandomNumberGenerator.
    /// </summary>
	[Export]
	private bool useSeed = true;
	/// <summary>
    /// Quantas iterações do algoritmo chamar.
    /// </summary>
	[Export]
	private int iterations = 3;

	/// <summary>
    /// Quantidade máxima de inimigos por sala.
    /// </summary>
	[ExportCategory("Monster RNG")]
	[Export]
	private int maxMonsterPerRoom = 2;

	public override void _Ready()
	{
		base._Ready();
		if (useSeed) {
			rng.Seed = seed;
		}
	}

	/// <summary>
    /// Transforma o tile da posição especificada em chão.
    /// </summary>
    /// <param name="data">o mapa</param>
    /// <param name="pos">posição para colocar o chão.</param>
	private static void CarveTile(MapData data, Vector2I pos)
	{
		Tile tile = data.GetTile(pos);
		if (tile == null) return;

		tile.SetDefinition(MapData.floorDefinition);
	}

	/// <summary>
    /// Preenche uma área retangular com chão.
    /// </summary>
    /// <param name="data">O mapa</param>
    /// <param name="room">Área para preencher com chão</param>
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

	/// <summary>
    /// Gera um andar da masmorra.
    /// Inimigos são colocados conforme configurações.
    /// O jogador é colocado na primeira sala gerada.
    /// </summary>
    /// <param name="player">Jogador.</param>
    /// <returns>O mapa gerado.</returns>
	public MapData GenerateDungeon(Player player)
	{
		MapData data = new MapData(width, height, player);

		// Divisão mestre que engloba o mapa inteiro.
		MapDivision root = new MapDivision(0, 0, width, height);

		// Chama o algoritmo para dividir o mapa.
		root.Split(iterations, rng);

		bool first = true;

		// Coloca os corredores.
		TunnelDivisions(data, root);

		// Cria as salas com base nas divisões geradas.
		foreach(MapDivision division in root.GetLeaves())
		{
			Rect2I room = new(division.Position, division.Size);
			
			// A sala não pode oculpar a divisão inteira, senão não haveriam paredes.
			room = room.GrowIndividual(
				-rng.RandiRange(1, 2),
				-rng.RandiRange(1, 2),
				-rng.RandiRange(1, 2),
				-rng.RandiRange(1, 2)
			);

			// De fato cria a sala.
			CarveRoom(data, room);
			// Colocamos o jogador na primeira sala.
			if (first)
			{
				first = false;
				player.GridPosition = room.GetCenter();
			}
			// Colocamos os inimigos na sala.
			PlaceEntities(data, room);
		}

		// Feito o mapa, inicializamos o algoritmo de pathfinding.
		data.SetupPathfinding();
		return data;
	}

	/// <summary>
    /// Popula uma sala com inimigos.
    /// </summary>
    /// <param name="data">O mapa</param>
    /// <param name="room">A sala.</param>
	private void PlaceEntities(MapData data, Rect2I room) {
		// Define quantos monstros serão colocados na sala
		int monsterAmount = rng.RandiRange(0, maxMonsterPerRoom);

		for (int i = 0; i < monsterAmount; i++) {
			// Escolhe um lugar aleatório na sala.
			Vector2I position = new(
				rng.RandiRange(room.Position.X, room.End.X - 1),
				rng.RandiRange(room.Position.Y, room.End.Y - 1)
			);

			// Só podemos colocar um ator por ponto no espaço.
			bool canPlace = true;
			foreach (Actor actor in data.Actors) {
				if (actor.GridPosition == position) {
					canPlace = false;
					break;
				}
			}

			// Se possível, criamos um inimigo aleatório na posição escolhida.
			if (canPlace) {
				EnemyDefinition definition = enemies.PickRandom();
				Enemy enemy = new Enemy(position, data, definition);
				data.InsertActor(enemy);
			}
		}
	}

	/// <summary>
    /// Preenche uma linha horizontal com chão.
    /// </summary>
    /// <param name="data">O mapa</param>
    /// <param name="y">Eixo y do corredor.</param>
    /// <param name="xBegin">Início do corredor</param>
    /// <param name="xEnd">Final do corredor.</param>
	private static void HorizontalCorridor(MapData data, int y, int xBegin, int xEnd) {
		int begin = (xBegin < xEnd) ? xBegin : xEnd;
		int end = (xEnd > xBegin) ? xEnd : xBegin;
		for (int i = begin; i <= end; i++) {
			CarveTile(data, new Vector2I(i, y));
		}
	}

	/// <summary>
    /// Preenche uma linha vertical com chão.
    /// </summary>
    /// <param name="data">O mapa.</param>
    /// <param name="x">Eixo x do corredor.</param>
    /// <param name="yBegin">Início do corredor</param>
    /// <param name="yEnd">Final do corredor.</param>
	private static void VerticalCorridor(MapData data, int x, int yBegin, int yEnd) {
		int begin = (yBegin < yEnd) ? yBegin : yEnd;
		int end = (yEnd > yBegin) ? yEnd : yBegin;
		for (int i = begin; i <= end; i++) {
			CarveTile(data, new Vector2I(x, i));
		}
	}

	/// <summary>
    /// Cria corredores vertical e horizontal para unir dois pontos no mapa.
    /// </summary>
    /// <param name="data">O mapa</param>
    /// <param name="start">Ponto inicial</param>
    /// <param name="end">Ponto final.</param>
	private void TunnelBetween(MapData data, Vector2I start, Vector2I end) {
		HorizontalCorridor(data, start.Y, start.X, end.X);
		VerticalCorridor(data, end.X, start.Y, end.Y);
	}

	/// <summary>
    /// Cria recursivamente corredores entre o centro de cada divisão do mapa.
    /// </summary>
    /// <param name="data">O mapa</param>
    /// <param name="root">Divisão mestre.</param>
	private void TunnelDivisions(MapData data, MapDivision root) {
		if (root.IsLeaf) {
			return;
		}

		TunnelBetween(data, root.Right.Center, root.Left.Center);
		TunnelDivisions(data, root.Left);
		TunnelDivisions(data, root.Right);
	}
}
