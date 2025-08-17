using Godot;
using System;

public abstract partial class Action : RefCounted {
	public abstract void Perform(Game game, Actor actor);
}
