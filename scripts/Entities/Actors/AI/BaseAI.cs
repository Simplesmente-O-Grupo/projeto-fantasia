using Godot;

namespace TheLegendOfGustav.Entities.Actors.AI;

/// <summary>
/// Enum das diferentes IAs disponíveis.
/// </summary>
public enum AIType
{
	None,
	DefaultHostile
};

/// <summary>
/// base para as IAs do jogo.
/// </summary>
public abstract partial class BaseAI : Node
{
	/// <summary>
	/// Corpo controlado pela IA.
	/// O corpo é a marionete da alma.
	/// </summary>
	protected Actor Body { get; set; }

	public override void _Ready()
	{
		base._Ready();
		// Por padrão, a IA é filha do nó de seu corpo.
		Body = GetParent<Actor>();
	}

	/// <summary>
	/// Computa um único turno para o ator controlado.
	/// Aviso: NPCs não possuem ações gratuitas.
	/// A IA SEMPRE precisa executar uma ação que custe energia.
	/// </summary>
	public abstract void Perform();

	/// <summary>
	/// Utiliza o pathfinder do mapa para obter um caminho
	/// da posição atual do ator para um destino qualquer.
	/// </summary>
	/// <param name="destination">Destino</param>
	/// <returns>Vetor com vetores, passo a passo para chegar no destino.</returns>
	public Godot.Collections.Array<Vector2> GetPathTo(Vector2I destination)
	{
		// Arrays do Godot são muito mais confortáveis de manipular, então
		// eu converto o Array do C# em um array do Godot antes de retornar o caminho.
		Godot.Collections.Array<Vector2> list = [];
		Vector2[] path = Body.MapData.Pathfinder.GetPointPath(Body.GridPosition, destination);
		list.AddRange(path);
		return list;
	}
}