using Godot;

/// <summary>
/// Classe para itens consumíveis.
/// Itens consumíveis são itens de uso limitado.
/// </summary>
public abstract partial class ConsumableItem : Entity
{
	public ConsumableItem(Vector2I initialPosition, MapData map, EntityDefinition definition) : base(initialPosition, map, definition)
	{
	}

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
	public abstract bool Activate(ItemAction action);

	public void ConsumedBy(Player consumer) {
		Inventory inventory = consumer.inventory;
		inventory.RemoveItem(this);
		QueueFree();
	}
}