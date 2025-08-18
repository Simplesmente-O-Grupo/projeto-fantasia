using Godot;
using System;
using System.Net.NetworkInformation;

public partial class MeleeAction : DirectionalAction
{
	public MeleeAction(Actor actor, Vector2I offset) : base(actor, offset)
	{
	}

	public override void Perform()
	{
		Vector2I destination = actor.GridPosition + Offset;

		Actor target = GetBlockingActorAtPosition(destination);

		if (target == null) return;

		GD.Print($"Você tenta socar {target.ActorName}, mas como não sobra nada para o beta, você ainda não tem um método de ataque.");
	}
}
