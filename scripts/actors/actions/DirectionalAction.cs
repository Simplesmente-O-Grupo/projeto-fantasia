using Godot;

/// <summary>
/// Ação direcionada. Esta ação é acompanhada com um vetor que representa uma
/// distância tendo como ponto de partida o ator.
/// </summary>
public abstract partial class DirectionalAction : Action
{
	/// <summary>
    /// Direção/distância do ator da ação.
    /// Seu significado depende da ação que implementará esta classe.
    /// </summary>
	public Vector2I Offset { get; private set; }
	public DirectionalAction(Actor actor, Vector2I offset) : base(actor)
	{
		Offset = offset;
	}

	/// <summary>
    /// É conveniente ter acesso à função para obter atores em uma determinada posição.
    /// Este método expõe o método de mesmo nome do mapa.
    /// </summary>
    /// <param name="pos">Posição para verificar</param>
    /// <returns>O ator naquela posição, nulo se não houver.</returns>
	protected Actor GetBlockingActorAtPosition(Vector2I pos) {
		return Map_Data.GetBlockingActorAtPosition(pos);
	}
}
