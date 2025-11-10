using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities;
using TheLegendOfGustav.Entities.Items;
using System;
using Godot.Collections;
using System.Diagnostics.Metrics;
using System.Numerics;

namespace TheLegendOfGustav.Map;

/// <summary>
/// A classe dungeonGenerator cria exatamente um andar da masmorra.
/// Ela é chamada quando necessário.
/// </summary>
public partial class DungeonGenerator : Node
{
	#region Fields
	/// <summary>
	/// Chave: Andar mínimo
	/// Valor: Número de máximo de monstros por sala
	/// </summary>
	private static readonly Dictionary<int, int> maxMonstersByFloor = new()
	{
		{1, 2},
		{4, 3},
		{6, 4},
		{10, 8}
	};
	/// <summary>
	/// Chave: Andar mínimo
	/// Valor: Número de máximo de itens por sala
	/// </summary>
	private static readonly Dictionary<int, int> maxItemsByFloor = new()
	{
		{1, 1},
		{4, 2},
		{6, 3},
	};
	/// <summary>
	/// Coleção de todos os inimigos que o gerador tem acesso.
	/// </summary>
	private static readonly Godot.Collections.Array<EnemyDefinition> enemies = [
		GD.Load<EnemyDefinition>("res://assets/definitions/actor/Skeleton.tres"),
		GD.Load<EnemyDefinition>("res://assets/definitions/actor/morcegao.tres"),
		GD.Load<EnemyDefinition>("res://assets/definitions/actor/Shadow.tres"),
		GD.Load<EnemyDefinition>("res://assets/definitions/actor/toilet.tres")
	];

	private static readonly Godot.Collections.Array<ItemResource> items = [
		GD.Load<ItemResource>("res://assets/definitions/Items/small_healing_potion.tres"),
		GD.Load<ItemResource>("res://assets/definitions/Items/mana_bolt_grimoire.tres"),
		GD.Load<ItemResource>("res://assets/definitions/Items/big_potion_of_heals.tres")
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
	/// Quantas iterações do algoritmo chamar.
	/// </summary>
	[Export]
	private int iterations = 3;

	/// <summary>
	/// Gerador de números aleatórios
	/// </summary>
	[ExportCategory("RNG")]
	private RandomNumberGenerator rng = new();
	#endregion

	#region Methods

	private int GetMaxIValueForFloor(Dictionary<int, int> valueTable, int currentFloor) {
		int currentValue = 0;

		int? key = null;

		foreach (int theKey in valueTable.Keys) {
			if (theKey > currentFloor) {
				break;
			} else {
				key = theKey;
			}
		}

		if (key.HasValue) {
			currentValue = valueTable[key.Value];
		}

		return currentValue;
	}

	/// <summary>
	/// Gera um andar da masmorra.
	/// Inimigos são colocados conforme configurações.
	/// O jogador é colocado na primeira sala gerada.
	/// </summary>
	/// <param name="player">Jogador.</param>
	/// <returns>O mapa gerado.</returns>
	public MapData GenerateDungeon(Player player, int currentFloor)
	{
		rng.Seed = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds();

		MapData data = new MapData(width, height, player);
		data.CurrentFloor = currentFloor;

		// Divisão mestre que engloba o mapa inteiro.
		MapDivision root = new MapDivision(0, 0, width, height);

		// Chama o algoritmo para dividir o mapa.
		root.Split(iterations, rng);

		bool first = true;

		// Coloca os corredores.
		TunnelDivisions(data, root);

		Vector2I lastRoom = new(0, 0);

		// Cria as salas com base nas divisões geradas.
		foreach (MapDivision division in root.GetLeaves())
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

			lastRoom = room.GetCenter();

			// Colocamos os inimigos na sala.
			PlaceEntities(data, room);
		}

		data.DownstairsLocation = lastRoom;
		Tile downTile = data.GetTile(data.DownstairsLocation);
		downTile.Key = TileType.DOWN_STAIRS;

		// Feito o mapa, inicializamos o algoritmo de pathfinding.
		data.SetupPathfinding();
		return data;
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

		tile.Key = TileType.FLOOR;
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
	/// Preenche uma linha horizontal com chão.
	/// </summary>
	/// <param name="data">O mapa</param>
	/// <param name="y">Eixo y do corredor.</param>
	/// <param name="xBegin">Início do corredor</param>
	/// <param name="xEnd">Final do corredor.</param>
	private static void HorizontalCorridor(MapData data, int y, int xBegin, int xEnd)
	{
		int begin = (xBegin < xEnd) ? xBegin : xEnd;
		int end = (xEnd > xBegin) ? xEnd : xBegin;
		for (int i = begin; i <= end; i++)
		{
			CarveTile(data, new Vector2I(i, y));
		}
	}

	/// <summary>
	/// Cria recursivamente corredores entre o centro de cada divisão do mapa.
	/// </summary>
	/// <param name="data">O mapa</param>
	/// <param name="root">Divisão mestre.</param>
	private static void TunnelDivisions(MapData data, MapDivision root)
	{
		if (root.IsLeaf)
		{
			return;
		}

		TunnelBetween(data, root.Right.Center, root.Left.Center);
		TunnelDivisions(data, root.Left);
		TunnelDivisions(data, root.Right);
	}

	/// <summary>
	/// Preenche uma linha vertical com chão.
	/// </summary>
	/// <param name="data">O mapa.</param>
	/// <param name="x">Eixo x do corredor.</param>
	/// <param name="yBegin">Início do corredor</param>
	/// <param name="yEnd">Final do corredor.</param>
	private static void VerticalCorridor(MapData data, int x, int yBegin, int yEnd)
	{
		int begin = (yBegin < yEnd) ? yBegin : yEnd;
		int end = (yEnd > yBegin) ? yEnd : yBegin;
		for (int i = begin; i <= end; i++)
		{
			CarveTile(data, new Vector2I(x, i));
		}
	}

	/// <summary>
	/// Cria corredores vertical e horizontal para unir dois pontos no mapa.
	/// </summary>
	/// <param name="data">O mapa</param>
	/// <param name="start">Ponto inicial</param>
	/// <param name="end">Ponto final.</param>
	private static void TunnelBetween(MapData data, Vector2I start, Vector2I end)
	{
		HorizontalCorridor(data, start.Y, start.X, end.X);
		VerticalCorridor(data, end.X, start.Y, end.Y);
	}

	/// <summary>
	/// Popula uma sala com inimigos.
	/// </summary>
	/// <param name="data">O mapa</param>
	/// <param name="room">A sala.</param>
	private void PlaceEntities(MapData data, Rect2I room)
	{
		// Define quantos monstros serão colocados na sala
		int monsterAmount = rng.RandiRange(0, GetMaxIValueForFloor(maxMonstersByFloor, data.CurrentFloor));
		// Define quantos itens serão colocados na sala.
		int itemAmount = rng.RandiRange(0, GetMaxIValueForFloor(maxItemsByFloor, data.CurrentFloor));

		for (int i = 0; i < monsterAmount; i++)
		{
			// Escolhe um lugar aleatório na sala.
			Vector2I position = new(
				rng.RandiRange(room.Position.X, room.End.X - 1),
				rng.RandiRange(room.Position.Y, room.End.Y - 1)
			);

			// Só podemos colocar um ator por ponto no espaço.
			bool canPlace = true;
			foreach (Entity entity in data.Entities)
			{
				if (entity.GridPosition == position)
				{
					canPlace = false;
					break;
				}
			}

			// Se possível, criamos um inimigo aleatório na posição escolhida.
			if (canPlace)
			{
				EnemyDefinition definition = enemies.PickRandom();
				Enemy enemy = new(position, data, definition);
				data.InsertEntity(enemy);
			}
		}

		for (int i = 0; i < itemAmount; i++)
		{
			// Escolhe um lugar aleatório na sala.
			Vector2I position = new(
				rng.RandiRange(room.Position.X, room.End.X - 1),
				rng.RandiRange(room.Position.Y, room.End.Y - 1)
			);

			bool canPlace = items.Count > 0;
			foreach (Entity entity in data.Entities)
			{
				if (entity.GridPosition == position)
				{
					canPlace = false;
					break;
				}
			}

			// Se possível, criamos um inimigo aleatório na posição escolhida.
			if (canPlace)
			{
				ItemResource itemRes = items.PickRandom();
				Item item = new(itemRes);
				ItemEntity itemEnt = new(position, data, item);
				data.InsertEntity(itemEnt);
			}
		}
	}
	#endregion
}
