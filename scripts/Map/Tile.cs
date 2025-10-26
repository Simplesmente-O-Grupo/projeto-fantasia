using System.Threading;
using Godot;
using Godot.Collections;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Map;

public enum TileType
{
	WALL,
	FLOOR
}

/// <summary>
/// O mundo do jogo é composto por Tiles.
/// Um tile é um quadrado de 16x16 que representa uma
/// unidade discreta do cenário. Tiles podem agir como
/// parede, chão, ou outras funções.
/// </summary>
public partial class Tile : Sprite2D, ISaveable
{
	private static readonly Godot.Collections.Dictionary<TileType, TileDefinition> Types = new()
	{
		{TileType.WALL, GD.Load<TileDefinition>("res://assets/definitions/tiles/wall.tres")},
		{TileType.FLOOR, GD.Load<TileDefinition>("res://assets/definitions/tiles/floor.tres")}
	};

	TileType key;
	public TileType Key
	{
		get => key;
		set
		{
			key = value;
			SetDefinition(Types[value]);
		}
	}

	private bool isExplored = false;
	private bool isInView = false;

	/// <summary>
	/// A definição do tile carrega seus valores padrão.
	/// </summary>
	private TileDefinition definition;

	public Tile(Vector2I pos, TileType type)
	{
		// Tile herda da classe Sprite2D.
		// Por padrão, a posição do Sprite2D é no centro de sua textura.
		// Para o jogo, faz mais sentido que a posição seja no 
		// canto superior esquerdo.
		Centered = false;
		// Tiles começam invisíveis porque não foram vistos pelo jogador.
		Visible = false;
		Position = Grid.GridToWorld(pos);
		Key = type;
	}

	/// <summary>
	/// Determina se atores podem andar em cima do Tile.
	/// </summary>
	public bool IsWalkable { get; private set; }
	/// <summary>
	/// Determina se o tile bloqueia visão.
	/// </summary>
	public bool IsTransparent { get; private set; }

	
	/// <summary>
	/// Se o jogador já viu este tile antes.
	/// Tiles não descobertos são invisíveis.
	/// </summary>
	public bool IsExplored
	{
		get => isExplored;
		set
		{
			isExplored = value;
			if (IsExplored && !Visible)
			{
				Visible = true;
			}
		}
	}

	/// <summary>
	/// Se o jogador vê o tile neste exato momento.
	/// Elementos neste tile estão dentro do campo de visão do jogador.
	/// </summary>
	public bool IsInView
	{
		get => isInView;
		set
		{
			isInView = value;
			Modulate = isInView ? definition.LitColor : definition.DarkColor;
			if (IsInView && !IsExplored)
			{
				IsExplored = true;
			}
		}
	}

	/// <summary>
	/// Define as características do tile.
	/// </summary>
	/// <param name="definition">Definição do tile.</param>
	public void SetDefinition(TileDefinition definition)
	{
		this.definition = definition;
		Modulate = definition.DarkColor;
		Texture = definition.Texture;
		IsWalkable = definition.IsWalkable;
		IsTransparent = definition.IsTransparent;
	}

	public Dictionary<string, Variant> GetSaveData()
	{
		return new()
		{
			{"key", (int)Key},
			{"is_explored", IsExplored}
		};
	}

	public bool LoadSaveData(Dictionary<string, Variant> saveData)
	{
		// É o seguinte, não tenho tempo, não vou verificar se a entrada está correta.
		Key = (TileType)(int)saveData["key"];

		IsExplored = (bool)saveData["is_explored"];
		return true;
	}
}
