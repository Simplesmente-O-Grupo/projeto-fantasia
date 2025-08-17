using Godot;
using System;
using System.Net.NetworkInformation;

public partial class MeleeAction : DirectionalAction
{
	public MeleeAction(Vector2I offset) : base(offset)
	{
	}

	public override void Perform(Game game, Actor actor)
	{
		Vector2I destination = actor.GridPosition + Offset;

		Actor target = game.Map.GetBlockingActorAtPosition(destination);

		if (target == null) return;

		GD.Print($"Você tenta socar {target.ActorName}, mas como não sobra nada para o beta, você ainda não tem um método de ataque.");
	}
}
