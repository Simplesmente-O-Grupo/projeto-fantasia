using Godot;
using System;
using System.Data;

public abstract partial class Action : RefCounted {
	protected Actor actor;

	public Action(Actor actor) {
		this.actor = actor;
	}

	public abstract void Perform();

	protected DungeonLevel Map {
		get => actor.Map;
	}
}
