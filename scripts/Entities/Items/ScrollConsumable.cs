using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.InputHandling;
using TheLegendOfGustav.Magic;
using TheLegendOfGustav.Map;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Items;

/// <summary>
/// Um pergaminho é um item consumível que joga exatamente um feitiço.
/// Feitiços de pergaminhos são gratuitos e não possuem restrições.
/// </summary>
/// <param name="initialPosition">Posição que o item será colocado no mapa.</param>
/// <param name="map">Mapa que o item será colocado.</param>
/// <param name="definition">Definição do item.</param>
public partial class ScrollConsumable(Vector2I initialPosition, MapData map, ScrollConsumableDefinition definition) : ConsumableItem(initialPosition, map, definition)
{
	public SpellResource Spell { get; private set; } = definition.Spell;

	/// <summary>
    /// O pergaminho só pode ser usado uma única vez.
    /// Alguns feitiços precisam de um alvo, o que significa que
    /// o pergaminho não pode se auto-destruir enquanto o jogador não
    /// escolher uma posição válida.
    /// 
    /// Esta variável garante que seja impossível de usar o pergaminho enquanto o jogador
    /// escolhe um alvo.
    /// </summary>
	private bool awaitingTermination = false;

	private ScrollConsumableDefinition definition = definition;

	/// <summary>
    /// O godot exige que desconectemos sinais customizados antes de 
    /// apagar nós.
    /// 
    /// Esta variável guarda a expressão lambda que usamos no sinal PlayerSpellCast
    /// para ser desconectado em ConsumedBy.
    /// </summary>
	private SignalBus.PlayerSpellCastEventHandler bindSignal = null;

	public override bool Activate(ItemAction action)
	{
		if (awaitingTermination)
		{
			return false;
		}
		// Feitiços de pergaminhos são gratuitos.
		Spell.Cost = 0;

		Player consumer = action.Player;

		// Alguns feitiços precisam de um alvo escolhido pelo jogador.
		// Não podemos esperar pelo jogador aqui, a função precisa retornar de forma síncrona.
		//
		// Então, se o feitiço precisar de um alvo, foi montada uma infraestrutura de sinais
		// para garantir que:
		// 1. O jogador possa escolher uma localização qualquer.
		// 2. Uma ação seja gerada com a posição escolhida e dentro de um inputHandler para entrar
		//    no ciclo de turnos normalmente.
		// 3. O pergaminho seja destruído quando o feitiço for executado com sucesso.
		if (Spell.Type == SpellType.Ranged)
		{
			// Este delegate existe somente para que eu possa desconectar o sinal de quando o feitiço é executado.
			// A engine exige que desconectemos sinais customizados antes de apagar um nó.
			bindSignal = delegate (bool success) { OnPlayerChoseTarget(success, action.Player); };
			
			// Eu mando dois sinais aqui. um deles avisa para o input handler trocar para o estado de
			// escolher posição de feitiço e o outro informa este inputhandler o feitiço deste pergaminho.
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.CommandInputHandler, (int)InputHandlers.CastSpell);
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.PlayerSpellChooseLocation, Spell);

			// Eu também conecto nosso delegate declarado anteriormente para quando o feitiço for executado.
			SignalBus.Instance.PlayerSpellCast += bindSignal;
			awaitingTermination = true;

			return true;
		}

		return true;
	}

	public override void ConsumedBy(Player consumer)
	{
		// De novo, a engine exige que desconectemos o sinal antes de destruir o pergaminho.
		if (bindSignal != null)
		{
			SignalBus.Instance.PlayerSpellCast -= bindSignal;
		}
		base.ConsumedBy(consumer);
	}

	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
		{
			if (bindSignal != null)
			{
				SignalBus.Instance.PlayerSpellCast -= bindSignal;
			}
		}
		base._Notification(what);
	}

	/// <summary>
	/// Este método é executado quando o feitiço deste pergaminho for executado
	/// (depois do jogador escolher um alvo.)
	/// </summary>
	/// <param name="success">Se o feitiço for executado com sucesso.</param>
	

	/// <summary>
    /// Este método é executado quando o feitiço deste pergaminho for executado
    /// (depois do jogador escolher um alvo.)
    /// </summary>
    /// <param name="success">Se o feitiço for executado com sucesso.</param>
    /// <param name="consumer">Quem ativou o feitiço.</param>
	private void OnPlayerChoseTarget(bool success, Player consumer) {
		if (success)
		{
			ConsumedBy(consumer);
		}
		else
		{
			// Se o feitiço não for ativado com sucesso,
			// podemos reutilizar o pergaminho.
			awaitingTermination = false;
		}
	}
}