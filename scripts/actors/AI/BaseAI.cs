using Godot;

/// <summary>
/// base para as IAs do jogo.
/// </summary>
public abstract partial class BaseAI : Node {
	/// <summary>
    /// Corpo controlado pela IA.
    /// O corpo é a marionete da alma.
    /// </summary>
	protected Actor body;

	public override void _Ready()
	{
		base._Ready();
		// Por padrão, a IA é filha do nó de seu corpo.
		body = GetParent<Actor>();
	}

	/// <summary>
    /// Computa um único turno para o ator controlado.
    /// </summary>
	public abstract void Perform();

	/// <summary>
    /// Utiliza o pathfinder do mapa para obter um caminho
    /// da posição atual do ator para um destino qualquer.
    /// </summary>
    /// <param name="destination">Destino</param>
    /// <returns>Vetor com vetores, passo a passo para chegar no destino.</returns>
	public Godot.Collections.Array<Vector2> GetPathTo(Vector2I destination) {
		// Arrays do Godot são muito mais confortáveis de manipular, então
		// eu converto o Array do C# em um array do Godot antes de retornar o caminho.
		Godot.Collections.Array<Vector2> list = [];
		Vector2[] path = body.Map_Data.Pathfinder.GetPointPath(body.GridPosition, destination);
		list.AddRange(path);
		return list;
	}
}