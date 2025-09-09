using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities.Actions;

namespace TheLegendOfGustav.InputHandling;

/// <summary>
/// Classe base para obter ações do usuário.
/// É interessante ter mais de um objeto para obter ações de
/// usuário porque permite limitar certas ações para
/// certos estados do jogo. Atualmente, o jogo
/// possui somente dois estados: Com jogador vivo e com jogador morto.
/// Mas isto pode aumentar.
/// </summary>
public abstract partial class BaseInputHandler : Node
{
	/// <summary>
	/// Método executado quando o input handler entra em cena;
	/// </summary>
	public virtual void Enter() { }
	/// <summary>
	/// Método executado quando o input handler sai de cena;
	/// </summary>
	public virtual void Exit() { }
	/// <summary>
	/// Obtém uma ação do usuári conforme input.
	/// </summary>
	/// <param name="player">Jogador</param>
	/// <returns>Ação que o jogador escolheu, nulo se nenhuma.</returns>
	public abstract Action GetAction(Player player);
}
