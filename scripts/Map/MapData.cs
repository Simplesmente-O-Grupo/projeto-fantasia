using Godot;
using TheLegendOfGustav.Entities;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities.Items;

namespace TheLegendOfGustav.Map;

/// <summary>
/// Classe que cuida dos dados e da parte lógica do mapa.
/// O mapa é o cenário onde as ações do jogo ocorrem.
/// Mais especificamente, o mapa é um único andar da masmorra.
/// </summary>
public partial class MapData : RefCounted
{
	#region Fields
	public static readonly TileDefinition wallDefinition = GD.Load<TileDefinition>("res://assets/definitions/tiles/wall.tres");
	public static readonly TileDefinition floorDefinition = GD.Load<TileDefinition>("res://assets/definitions/tiles/floor.tres");
	/// <summary>
	/// Peso do ator no pathfinder.
	/// A IA irá evitar de passar por espaços com peso alto.
	/// </summary>
	private static readonly float entityWeight = 10.0f;
	#endregion

	#region Constructor
	public MapData(int width, int height, Player player)
	{
		Width = width;
		Height = height;

		Player = player;
		// Como o jogador é criado antes do mapa, precisamos
		// atualizá-lo com o novo mapa.
		player.MapData = this;
		InsertEntity(player);

		SetupTiles();
	}
	#endregion

	#region Signals
	[Signal]
	public delegate void EntityPlacedEventHandler(Entity entity);
	#endregion

	#region Properties
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
	/// Lista de todas as entidades dentro do mapa.
	/// </summary>
	public Godot.Collections.Array<Entity> Entities { get; private set; } = [];

	/// <summary>
    /// Lista de todos os itens dentro do mapa.
    /// </summary>
	public Godot.Collections.Array<ItemEntity> Items
	{
		get
		{
			Godot.Collections.Array<ItemEntity> list = [];
			foreach (Entity entity in Entities)
			{
				if (entity is ItemEntity item)
				{
					list.Add(item);
				}
			}
			return list;
		}
	}
	/// <summary>
	/// Objeto do Godot que utiliza do algoritmo A* para calcular
	/// caminhos e rotas.
	/// </summary>
	public AStarGrid2D Pathfinder { get; private set; }
	#endregion

	#region Methods
	/// <summary>
	/// Inicializa o pathfinder;
	/// </summary>
	public void SetupPathfinding()
	{
		Pathfinder = new AStarGrid2D
		{
			//A região é o mapa inteiro.
			Region = new Rect2I(0, 0, Width, Height)
		};

		// Atualiza o pathfinder para a região definida.
		Pathfinder.Update();

		// Define quais pontos do mapa são passáveis ou não.
		for (int y = 0; y < Height; y++)
		{
			for (int x = 0; x < Width; x++)
			{
				Vector2I pos = new Vector2I(x, y);
				Tile tile = GetTile(pos);
				// Pontos sólidos são impossíveis de passar.
				Pathfinder.SetPointSolid(pos, !tile.IsWalkable);
			}
		}

		// Registra todos os atores em cena.
		foreach (Entity entity in Entities)
		{
			if (entity.BlocksMovement)
			{
				RegisterBlockingEntity(entity);
			}
		}

	}

	/// <summary>
	/// Define um peso na posição de uma entidade para que a IA evite de passar por lá.
	/// Ênfase em evitar. Se  o único caminho para o destino estiver bloqueado
	/// por uma entidade, o jogo tentará andar mesmo assim.
	/// </summary>
	/// <param name="entity">A entidade em questão.</param>
	public void RegisterBlockingEntity(Entity entity)
	{
		Pathfinder.SetPointWeightScale(entity.GridPosition, entityWeight);
	}

	/// <summary>
	/// Remove o peso na posição de uma entidade.
	/// Quando uma entidade move sua posição, devemos tirar o peso de sua posição anterior.
	/// </summary>
	/// <param name="entity">A entidade em questão.</param>
	public void UnregisterBlockingEntity(Entity entity)
	{
		Pathfinder.SetPointWeightScale(entity.GridPosition, 0);
	}

	/// <summary>
	/// Registra uma entidade no mapa. A existência de uma entidade não é considerada se ela não
	/// estiver registrada no mapa.
	/// </summary>
	/// <param name="entity">A entidade em questão</param>
	public void InsertEntity(Entity entity)
	{
		Entities.Add(entity);
	}

	/// <summary>
	/// Obtém o tile na posição desejada.
	/// </summary>
	/// <param name="pos">Vetor posição</param>
	/// <returns>O tile na posição, nulo se for fora do mapa.</returns>
	public Tile GetTile(Vector2I pos)
	{
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
	public Tile GetTile(int x, int y)
	{
		return GetTile(new Vector2I(x, y));
	}
		/// <summary>
	/// Obtém a entidade na posição especificada.
	/// </summary>
	/// <param name="pos">Vetor posição</param>
	/// <returns>A entidade na posição especificada, nulo se não houver.</returns>
	public Entity GetBlockingEntityAtPosition(Vector2I pos)
	{
		foreach (Entity entity in Entities)
		{
			if (entity.GridPosition == pos && entity.BlocksMovement)
			{
				return entity;
			}
		}
		return null;
	}

	/// <summary>
	/// Obtém o primeiro item na posição especificada.
	/// </summary>
	/// <param name="pos">Posição</param>
	/// <returns>O primeiro item na posição, nulo se não houver.</returns>
	public ItemEntity GetFirstItemAtPosition(Vector2I pos)
	{
		foreach (ItemEntity item in Items)
		{
			if (item.GridPosition == pos)
			{
				return item;
			}
		}

		return null;
	}

	/// <summary>
	/// Remove uma entidade do mapa sem dar free.
	/// </summary>
	/// <param name="entity">A entidade para remover</param>
	public void RemoveEntity(Entity entity)
	{
		// Eu removo a entidade do nó de entidades.
		entity.GetParent().RemoveChild(entity);
		// Eu removo a entidade da lista de entidades do mapa.
		Entities.Remove(entity);
	}

	/// <summary>
	/// Obtém todas as entidades na posição especificada.
	/// É possível haver mais de uma entidade na mesma posição se uma delas não bloquear movimento.
	/// </summary>
	/// <param name="pos">Vetor posição</param>
	/// <returns>Lista com todas as entidades na posição especificada.</returns>
	public Godot.Collections.Array<Entity> GetEntitiesAtPosition(Vector2I pos)
	{
		Godot.Collections.Array<Entity> ZOfZero = [];
		Godot.Collections.Array<Entity> ZOfOne = [];
		Godot.Collections.Array<Entity> ZOfTwo = [];

		// Pego todos os atores
		foreach (Entity entity in Entities)
		{
			if (entity.GridPosition == pos)
			{
				switch (entity.ZIndex)
				{
					case 0:
						ZOfZero.Add(entity);
						break;
					case 1:
						ZOfOne.Add(entity);
						break;
					case 2:
						ZOfTwo.Add(entity);
						break;
				}
			}
		}

		// Retorno os atores ordenados por ZIndex.
		return ZOfZero + ZOfOne + ZOfTwo;
	}

	/// <summary>
	/// Cria novos Tiles até preencher as dimensões do mapa.
	/// É importante que estes tiles sejam paredes, o gerador de mapas
	/// não cria paredes por conta própria.
	/// </summary>
	private void SetupTiles()
	{
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				Tiles.Add(new Tile(new Vector2I(j, i), wallDefinition));
			}
		}
	}

	/// <summary>
	/// Converte uma coordenada em um índice para acessar a lista de tiles.
	/// </summary>
	/// <param name="pos">Vetor posição</param>
	/// <returns>Índice na lista de tiles. -1 se estiver fora do mapa.</returns>
	private int GridToIndex(Vector2I pos)
	{
		if (!IsInBounds(pos)) return -1;

		return pos.Y * Width + pos.X;
	}

	/// <summary>
	/// Se uma coordenada está dentro da área do mapa.
	/// </summary>
	/// <param name="pos">Vetor posição</param>
	/// <returns>Se o vetor está dentro do mapa.</returns>
	private bool IsInBounds(Vector2I pos)
	{
		if (pos.X < 0 || pos.Y < 0)
		{
			return false;
		}
		if (pos.X >= Width || pos.Y >= Height)
		{
			return false;
		}

		return true;
	}
	#endregion
}
