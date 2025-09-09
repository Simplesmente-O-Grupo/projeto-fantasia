using Godot;
using TheLegendOfGustav.Entities;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.Map;

/// <summary>
/// A parte visual do mapa.
/// </summary>
public partial class Map : Node2D
{
	/// <summary>
	/// Dados do mapa.
	/// </summary>
	public MapData MapData { get; private set; }

	/// <summary>
	/// raio de alcance da visão do jogador.
	/// </summary>
	[Export]
	private int FovRadius { get; set; } = 12;

	/// <summary>
	/// Gerador de mapas.
	/// </summary>
	private DungeonGenerator Generator { get; set; }

	FieldOfView FieldOfView { get; set; }

	private Node2D TilesNode { get; set; }
	private Node2D EntitiesNode { get; set; }

	public override void _Ready()
	{
		base._Ready();
		// Começamos obtendo nós relevantes para o mapa.
		Generator = GetNode<DungeonGenerator>("Generator");
		FieldOfView = GetNode<FieldOfView>("FieldOfView");
		TilesNode = GetNode<Node2D>("Tiles");
		EntitiesNode = GetNode<Node2D>("Entities");
	}

	/// <summary>
	/// Cria um andar da masmorra utilizando o gerador de mapa.
	/// </summary>
	/// <param name="player">O gerador de mapas precisa do jogador.</param>
	public void Generate(Player player)
	{
		MapData = Generator.GenerateDungeon(player);

		MapData.EntityPlaced += OnEntityPlaced;

		PlaceTiles();
		PlaceEntities();
	}

	/// <summary>
	/// Atualiza o campo de visão do mapa com base em uma coordenada.
	/// </summary>
	/// <param name="pos">Centro de visão, normalmente é a posição do jogador.</param>
	public void UpdateFOV(Vector2I pos)
	{
		FieldOfView.UpdateFOV(MapData, pos, FovRadius);
		// Esconde ou revela entidades com base no campo de visão.
		foreach (Entity entity in MapData.Entities)
		{
			entity.Visible = MapData.GetTile(entity.GridPosition).IsInView;
		}
	}


	/// <summary>
	/// Coloca todos os tiles do mapa no mundo do jogo.
	/// </summary>
	private void PlaceTiles()
	{
		foreach (Tile tile in MapData.Tiles)
		{
			TilesNode.AddChild(tile);
		}
	}

	/// <summary>
	/// Coloca todas as entidades do mapa no mundo do jogo.
	/// </summary>
	private void PlaceEntities()
	{
		foreach (Entity entity in MapData.Entities)
		{
			EntitiesNode.AddChild(entity);
		}
	}

	private void OnEntityPlaced(Entity entity)
	{
		EntitiesNode.AddChild(entity);
	}
}
