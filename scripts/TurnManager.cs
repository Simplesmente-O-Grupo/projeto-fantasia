using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

public partial class TurnManager : Node {
	[Signal]
	public delegate void turnBeginEventHandler();
	[Signal]
	public delegate void turnEndEventHandler();

	private Godot.Collections.Array<Node> actors = [];
	private int index = 0;

	public int TurnCount { get; private set; } = 1;

	public void Tick() {
		EmitSignal(SignalName.turnBegin);

		GD.Print("Turn: " + TurnCount);

		actors.Clear();
		actors = GetTree().GetNodesInGroup("TimeSlave");

		GD.Print("Actor count: " + actors.Count);

		index = -1;
		NextActor();
	}

	private void NextActor() {
		index++;
		GD.Print("Index: " + index);
		if (index >= actors.Count) {
			EndTurn();
			return;
		}
		Actor currentActor = (Actor) actors[index];
		currentActor.Energy += currentActor.Speed;
		ActorPerformAction();
	}

	private void ActorPerformAction() {
		Actor currentActor = (Actor) actors[index];
		if (currentActor.Energy > 0) {
			currentActor.performAction();
		} else {
			NextActor();
		}
	}

	public void OnActionEnd() {
		ActorPerformAction();
	}

	private void EndTurn() {
		GD.Print("Turn End");
		TurnCount++;
		EmitSignal(SignalName.turnEnd);
	}
}
