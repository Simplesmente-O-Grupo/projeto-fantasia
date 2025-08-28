using Godot;
using System;

/// <summary>
/// O mundo do jogo é composto por Tiles.
/// Um tile é um quadrado de 16x16 que representa uma
/// unidade discreta do cenário. Tiles podem agir como
/// parede, chão, ou outras funções.
/// </summary>
public partial class Tile : Sprite2D
{
	/// <summary>
    /// A definição do tile carrega seus valores padrão.
    /// </summary>
	private TileDefinition definition;

	/// <summary>
    /// Determina se atores podem andar em cima do Tile.
    /// </summary>
	public bool IsWalkable { get; private set; }
	/// <summary>
    /// Determina se o tile bloqueia visão.
    /// </summary>
	public bool IsTransparent { get; private set; }

	private bool isExplored = false;
	/// <summary>
    /// Se o jogador já viu este tile antes.
    /// Tiles não descobertos são invisíveis.
    /// </summary>
	public bool IsExplored {
		get => this.isExplored;
		set {
			isExplored = value;
			if (IsExplored && !Visible) {
				Visible = true;
			}
		}
	}

	private bool isInView = false;
	/// <summary>
    /// Se o jogador vê o tile neste exato momento.
    /// Elementos neste tile estão dentro do campo de visão do jogador.
    /// </summary>
	public bool IsInView {
		get => this.isInView;
		set {
			this.isInView = value;
			if (IsInView && !IsExplored) {
				IsExplored = true;
			}
		}
	}

	public Tile(Vector2I pos, TileDefinition definition)
	{
		// Tile herda da classe Sprite2D.
		// Por padrão, a posição do Sprite2D é no centro de sua textura.
		// Para o jogo, faz mais sentido que a posição seja no 
		// canto superior esquerdo.
		Centered = false;
		// Tiles começam invisíveis porque não foram vistos pelo jogador.
		Visible = false;
		Position = Grid.GridToWorld(pos);
		SetDefinition(definition);
	}

	/// <summary>
    /// Define as características do tile.
    /// </summary>
    /// <param name="definition">Definição do tile.</param>
	public void SetDefinition(TileDefinition definition) {
		this.definition = definition;
		Texture = definition.Texture;
		IsWalkable = definition.IsWalkable;
		IsTransparent = definition.IsTransparent;
	}
}
