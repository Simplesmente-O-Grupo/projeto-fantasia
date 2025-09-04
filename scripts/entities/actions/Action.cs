using Godot;


/// <summary>
///  <c>Action</c> representa uma ação no jogo efetuada por um ator.
///  Ações são geradas pelo jogador e pela IA, elas regem os atores do jogo.
/// </summary>
public abstract partial class Action : RefCounted {
	/// <summary>
    /// O ator que realiza a ação.
    /// </summary>
	protected Actor actor;

    // O custo da ação.
	protected int cost;

	public Action(Actor actor) {
		this.actor = actor;
		// Custo base, subclasses podem sobreescrever isto se quiserem.
		cost = 10;
	}

	/// <summary>
    /// Método que executa a ação. Subclasses da ação devem implementar este método.
    /// <example>
    /// Exemplo:
    /// <code>
    /// Action action = new Action(actor);
    /// /* . . . */
    /// action.Perform();
    /// </code>
    /// </example>
    /// </summary>
    /// <returns>Se a ação foi executada ou não.</returns>
	public abstract bool Perform();

	/// <summary>
    /// É conveniente ter acesso ao mapa dentro de uma ação.
    /// </summary>
	protected MapData Map_Data {
		get => actor.Map_Data;
	}
}
