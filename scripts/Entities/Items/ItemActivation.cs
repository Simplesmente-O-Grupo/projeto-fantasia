using Godot;
using TheLegendOfGustav.Entities.Actors;

/// <summary>
/// Isto é um pedido de ajuda. Estou gritando ao vazio e o vazio permanece em silêncio.
/// Eu quero que itens tenham efeitos arbitrários e quero que sejam fáceis de serializar.
/// Este é a única forma que encontrei para tornar isto possível.
/// </summary>
[GlobalClass]
public abstract partial class ItemActivation : Resource
{
	public abstract bool OnActivation(Player consumer);
}