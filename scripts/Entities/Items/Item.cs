using System.Reflection.Metadata;
using Godot;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.Entities.Items;

public partial class Item : RefCounted
{

	public Item(ItemResource definition)
	{
		Definition = definition;
		Uses = Definition.MaxUses;
	}

	public ItemResource Definition { get; private set; }
	public int Uses { get; set; }

	/// <summary>
	/// Gera uma ação onde o ator consome o item.
	/// </summary>
	/// <param name="consumer"></param>
	/// <returns></returns>
	public Action GetAction(Player consumer)
	{
		return new ItemAction(consumer, this);
	}

	/// <summary>
	/// Ativa a função deste item.
	/// Este método é chamado pela ação gerada por ele mesmo.
	/// Este método permite definir condições para a sua ativação.
	/// </summary>
	/// <param name="action">Ação gerada pelo item.</param>
	/// <returns>Se a ação foi realizada ou não.</returns>
	public bool Activate(ItemAction action)
	{
		if (Uses != 0) 
		{
			bool ret = Definition.Activation.OnActivation(action.Player);

			if (ret && Uses > 0)
			{
				Uses--;
			}

			return ret;
		}
		else 
		{
			return false;
		}
	}

	public virtual void ConsumedBy(Player consumer)
	{
		Inventory inventory = consumer.Inventory;
		inventory.RemoveItem(this);
	}
}