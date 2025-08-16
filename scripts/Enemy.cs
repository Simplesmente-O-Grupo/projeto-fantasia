using Godot;
using System;

public partial class Enemy : Actor {
	public override void PerformAction() {
		Walk(Vector2I.Right);
	}
}
