using System.Net.Http.Headers;
using Godot;
using TheLegendOfGustav.Entities;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Map;

/// <summary>
/// A parte visual do mapa.
/// </summary>
public partial class Map : Node2D
{
	/// <summary>
	/// raio de alcance da visão do jogador.
	/// </summary>
	[Export]
	private int fovRadius = 12;

	private FieldOfView fieldOfView;

	private Node2D tilesNode;
	private Node2D entitiesNode;

	/// <summary>
	/// Gerador de mapas.
	/// </summary>
	private DungeonGenerator generator;

	/// <summary>
	/// Dados do mapa.
	/// </summary>
	public MapData MapData { get; private set; }

	[Signal]
	public delegate void DungeonFloorChangedEventHandler(int floor);

	private SignalBus.PlayerDescentEventHandler joinSignal;
	public override void _Ready()
	{
		base._Ready();
		// Começamos obtendo nós relevantes para o mapa.
		generator = GetNode<DungeonGenerator>("Generator");
		fieldOfView = GetNode<FieldOfView>("FieldOfView");
		tilesNode = GetNode<Node2D>("Tiles");
		entitiesNode = GetNode<Node2D>("Entities");

		joinSignal = () => NextFloor();
		SignalBus.Instance.PlayerDescent += joinSignal;
	}

	void NextFloor()
	{
		Player player = MapData.Player;
		entitiesNode.RemoveChild(player);

		foreach (var entity in entitiesNode.GetChildren())
		{
			entity.QueueFree();
		}

		foreach (var tile in tilesNode.GetChildren())
		{
			tile.QueueFree();
		}

		Generate(player, MapData.CurrentFloor + 1);
		player.GetNode<Camera2D>("Camera2D").MakeCurrent();
		fieldOfView.ResetFOV();
		UpdateFOV(player.GridPosition);
	}

	/// <summary>
	/// Cria um andar da masmorra utilizando o gerador de mapa.
	/// </summary>
	/// <param name="player">O gerador de mapas precisa do jogador.</param>
	public void Generate(Player player, int currentFloor = 1)
	{
		MapData = generator.GenerateDungeon(player, currentFloor);

		MapData.EntityPlaced += OnEntityPlaced;

		PlaceTiles();
		PlaceEntities();

		EmitSignal(SignalName.DungeonFloorChanged, currentFloor);
	}

	/// <summary>
	/// Atualiza o campo de visão do mapa com base em uma coordenada.
	/// </summary>
	/// <param name="pos">Centro de visão, normalmente é a posição do jogador.</param>
	public void UpdateFOV(Vector2I pos)
	{
		fieldOfView.UpdateFOV(MapData, pos, fovRadius);
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
			tilesNode.AddChild(tile);
		}
	}

	/// <summary>
	/// Coloca todas as entidades do mapa no mundo do jogo.
	/// </summary>
	private void PlaceEntities()
	{
		foreach (Entity entity in MapData.Entities)
		{
			entitiesNode.AddChild(entity);
		}
	}

	private void OnEntityPlaced(Entity entity)
	{
		entitiesNode.AddChild(entity);
	}

	public bool LoadGame(Player player)
	{

		MapData = new(0, 0, player);

		if (!MapData.LoadGame())
		{
			return false;
		}

		PlaceTiles();
		PlaceEntities();

		MapData.EntityPlaced += OnEntityPlaced;
		EmitSignal(SignalName.DungeonFloorChanged, MapData.CurrentFloor);
		return true;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
		{
			if (joinSignal != null)
			{
				SignalBus.Instance.PlayerDescent -= joinSignal;
			}
		}
		base._Notification(what);
	}
}
