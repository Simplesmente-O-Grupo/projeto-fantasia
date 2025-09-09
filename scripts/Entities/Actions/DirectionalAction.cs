using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Map;

namespace TheLegendOfGustav.Entities.Actions;

/// <summary>
/// Ação direcionada. Esta ação é acompanhada com um vetor que representa uma
/// distância tendo como ponto de partida o ator.
/// </summary>
public abstract partial class DirectionalAction : Action
{
	public DirectionalAction(Actor actor, Vector2I offset) : base(actor)
	{
		Offset = offset;
	}

	/// <summary>
	/// Direção/distância do ator da ação.
	/// Seu significado depende da ação que implementará esta classe.
	/// </summary>
	public Vector2I Offset { get; private set; }

	/// <summary>
	/// Coordenada do alvo da ação.
	/// </summary>
	public Vector2I Destination { get => Actor.GridPosition + Offset; }

	/// <summary>
	/// Função que obtém o alvo da ação, se houver.
	/// </summary>
	/// <returns>O ator alvo da ação, nulo se não houver.</returns>
	protected Entity GetTarget()
	{
		return MapData.GetBlockingEntityAtPosition(Destination);
	}
}
