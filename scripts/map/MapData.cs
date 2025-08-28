using Godot;

/// <summary>
/// Classe que cuida dos dados e da parte lógica do mapa.
/// O mapa é o cenário onde as ações do jogo ocorrem.
/// Mais especificamente, o mapa é um único andar da masmorra.
/// </summary>
public partial class MapData : RefCounted
{
	public static readonly TileDefinition wallDefinition = GD.Load<TileDefinition>("res://assets/definitions/tiles/wall.tres");
	public static readonly TileDefinition floorDefinition = GD.Load<TileDefinition>("res://assets/definitions/tiles/floor.tres");

	/// <summary>
    /// Largura do mapa.
    /// </summary>
	public int Width { get; private set; }
	/// <summary>
    /// Altura do mapa.
    /// </summary>
	public int Height { get; private set; }

	/// <summary>
    /// Os tiles que compõem o mapa.
    /// </summary>
	public Godot.Collections.Array<Tile> Tiles { get; private set; } = [];

	/// <summary>
    /// O jogador é especial e por isso o mapa faz questão de rastreá-lo.
    /// </summary>
	public Player Player { get; set; }
	/// <summary>
    /// Lista de todos os atores dentro do mapa.
    /// </summary>
	public Godot.Collections.Array<Actor> Actors { get; private set; } = [];

	private AStarGrid2D pathfinder;
	/// <summary>
    /// Objeto do Godot que utiliza do algoritmo A* para calcular
    /// caminhos e rotas.
    /// </summary>
	public AStarGrid2D Pathfinder { get => pathfinder; }
	/// <summary>
    /// Peso do ator no pathfinder.
    /// A IA irá evitar de passar por espaços com peso alto.
    /// </summary>
	private static float ActorWeight = 10.0f;

	/// <summary>
    /// Inicializa o pathfinder;
    /// </summary>
	public void SetupPathfinding() {
		pathfinder = new AStarGrid2D
		{
			//A região é o mapa inteiro.
			Region = new Rect2I(0, 0, Width, Height)
		};

		// Atualiza o pathfinder para a região definida.
		pathfinder.Update();

		// Define quais pontos do mapa são passáveis ou não.
		for (int y = 0; y < Height; y++) {
			for (int x = 0; x < Width; x++) {
				Vector2I pos = new Vector2I(x, y);
				Tile tile = GetTile(pos);
				// Pontos sólidos são impossíveis de passar.
				pathfinder.SetPointSolid(pos, !tile.IsWalkable);
			}
		}

		// Registra todos os atores em cena.
		foreach (Actor actor in Actors) {
			if (actor.BlocksMovement) {
				RegisterBlockingActor(actor);
			}
		}

	}

	/// <summary>
    /// Define um peso na posição de um ator para que a IA evite de passar por lá.
    /// Ênfase em evitar. Se  o único caminho para o destino estiver bloqueado
    /// por um ator, o jogo tentará andar mesmo assim.
    /// </summary>
    /// <param name="actor">O ator em questão.</param>
	public void RegisterBlockingActor(Actor actor) {
		pathfinder.SetPointWeightScale(actor.GridPosition, ActorWeight);
	}

	/// <summary>
    /// Remove o peso na posição de um ator.
    /// Quando um ator move sua posição, devemos tirar o peso de sua posição anterior.
    /// </summary>
    /// <param name="actor">O ator em questão.</param>
	public void UnregisterBlockingActor(Actor actor) {
		pathfinder.SetPointWeightScale(actor.GridPosition, 0);
	}

	public MapData(int width, int height, Player player) {
		Width = width;
		Height = height;

		Player = player;
		// Como o jogador é criado antes do mapa, precisamos
		// atualizá-lo com o novo mapa.
		player.Map_Data = this;
		InsertActor(player);

		SetupTiles();
	}

	/// <summary>
    /// Cria novos Tiles até preencher as dimensões do mapa.
    /// É importante que estes tiles sejam paredes, o gerador de mapas
    /// não cria paredes por conta própria.
    /// </summary>
	private void SetupTiles() {
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				Tiles.Add(new Tile(new Vector2I(j, i), wallDefinition));
			}
		}
	}

	/// <summary>
    /// Registra um ator no mapa. A existência de um ator não é considerada se ele não
    /// estiver registrado no mapa.
    /// </summary>
    /// <param name="actor">O ator em questão</param>
	public void InsertActor(Actor actor) {
		Actors.Add(actor);
	}

	/// <summary>
    /// Converte uma coordenada em um índice para acessar a lista de tiles.
    /// </summary>
    /// <param name="pos">Vetor posição</param>
    /// <returns>Índice na lista de tiles. -1 se estiver fora do mapa.</returns>
	private int GridToIndex(Vector2I pos) {
		if (!IsInBounds(pos)) return -1;

		return pos.Y * Width + pos.X;
	}

	/// <summary>
    /// Se uma coordenada está dentro da área do mapa.
    /// </summary>
    /// <param name="pos">Vetor posição</param>
    /// <returns>Se o vetor está dentro do mapa.</returns>
	private bool IsInBounds(Vector2I pos) {
		if (pos.X < 0 || pos.Y < 0) {
			return false;
		}
		if (pos.X >= Width || pos.Y >= Height) {
			return false;
		}

		return true;
	}

	/// <summary>
    /// Obtém o tile na posição desejada.
    /// </summary>
    /// <param name="pos">Vetor posição</param>
    /// <returns>O tile na posição, nulo se for fora do mapa.</returns>
	public Tile GetTile(Vector2I pos) {
		int index = GridToIndex(pos);

		if (index < 0) return null;

		return Tiles[index];
	}

	/// <summary>
    /// Obtém o tile na posição desejada.
    /// </summary>
    /// <param name="x">x da coordenada</param>
    /// <param name="y">y da coordenada</param>
    /// <returns>O tile na posição, nulo se for fora do mapa.</returns>
	public Tile GetTile(int x, int y) {
		return GetTile(new Vector2I(x, y));
	}

	/// <summary>
    /// Obtém o ator na posição especificada.
    /// </summary>
    /// <param name="pos">Vetor posição</param>
    /// <returns>O ator na posição especificada, nulo se não houver.</returns>
	public Actor GetBlockingActorAtPosition(Vector2I pos) {
		foreach (Actor actor in Actors) {
			if (actor.GridPosition == pos && actor.BlocksMovement) {
				return actor;
			}
		}
		return null;
	}

	/// <summary>
    /// Verifica se é possível caminhar na coordenada especificada.
    /// Este método será removido.
    /// </summary>
    /// <param name="pos">Vetor posição</param>
    /// <returns>Se é possível caminhar nesta posição</returns>
	public bool IsTileWalkable(Vector2I pos) {
		Tile tile = GetTile(pos);

		if (tile == null) return false;

		return tile.IsWalkable;
	}
}
