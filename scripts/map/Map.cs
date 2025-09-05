using Godot;

/// <summary>
/// A parte visual do mapa.
/// </summary>
public partial class Map : Node2D
{
	/// <summary>
    /// Dados do mapa.
    /// </summary>
	public MapData Map_Data { get; private set; }

	/// <summary>
    /// raio de alcance da visão do jogador.
    /// </summary>
	[Export]
	private int fovRadius = 12;

	/// <summary>
    /// Gerador de mapas.
    /// </summary>
	private DungeonGenerator generator;

	FieldOfView fieldOfView;

	private Node2D tilesNode;
	private Node2D entitiesNode;

	public override void _Ready()
	{
		base._Ready();
		// Começamos obtendo nós relevantes para o mapa.
		generator = GetNode<DungeonGenerator>("Generator");
		fieldOfView = GetNode<FieldOfView>("FieldOfView");
		tilesNode = GetNode<Node2D>("Tiles");
		entitiesNode = GetNode<Node2D>("Entities");
	}

	/// <summary>
    /// Coloca todos os tiles do mapa no mundo do jogo.
    /// </summary>
	private void PlaceTiles() {
		foreach (Tile tile in Map_Data.Tiles) {
			tilesNode.AddChild(tile);
		}
	}

	/// <summary>
    /// Coloca todas as entidades do mapa no mundo do jogo.
    /// </summary>
	private void PlaceEntities() {
		foreach (Entity entity in Map_Data.Entities) {
			entitiesNode.AddChild(entity);
		}
	}

	/// <summary>
    /// Cria um andar da masmorra utilizando o gerador de mapa.
    /// </summary>
    /// <param name="player">O gerador de mapas precisa do jogador.</param>
	public void Generate(Player player)
	{
		Map_Data = generator.GenerateDungeon(player);

		Map_Data.EntityPlaced += OnEntityPlaced;

		PlaceTiles();
		PlaceEntities();
	}

	/// <summary>
    /// Atualiza o campo de visão do mapa com base em uma coordenada.
    /// </summary>
    /// <param name="pos">Centro de visão, normalmente é a posição do jogador.</param>
	public void UpdateFOV(Vector2I pos) {
		fieldOfView.UpdateFOV(Map_Data, pos, fovRadius);
		// Esconde ou revela entidades com base no campo de visão.
		foreach (Entity entity in Map_Data.Entities) {
			entity.Visible = Map_Data.GetTile(entity.GridPosition).IsInView;
		}
	}

	private void OnEntityPlaced(Entity entity) {
		entitiesNode.AddChild(entity);
	}
}
