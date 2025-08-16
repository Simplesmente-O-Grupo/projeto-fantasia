using Godot;
using System;

public partial class Enemy : Actor {
	public override void performAction() {
		Walk(Vector2I.Right);

		GD.Print("Energy after walking: " + Energy);
	}
}
