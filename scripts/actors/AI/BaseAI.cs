using Godot;

public abstract partial class BaseAI : Node {
	protected Actor body;

	public override void _Ready()
	{
		base._Ready();
		body = GetParent<Actor>();
	}

	public abstract void Perform();

	public Godot.Collections.Array<Vector2> GetPathTo(Vector2I destination) {
		Godot.Collections.Array<Vector2> list = [];
		Vector2[] path = body.Map_Data.Pathfinder.GetPointPath(body.GridPosition, destination);
		list.AddRange(path);
		return list;
	}
}