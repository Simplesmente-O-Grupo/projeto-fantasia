using Godot;

public partial class HostileEnemyAI : BaseAI
{
	private Godot.Collections.Array<Vector2> path = [];

	public override void Perform()
	{
		Player target = body.Map_Data.Player;
		Vector2I offset = target.GridPosition - body.GridPosition;
		int distance = int.Max(int.Abs(offset.X), int.Abs(offset.Y));

		Action action;

		if (body.Map_Data.GetTile(body.GridPosition).IsInView) {
			if (distance <= 1) {
				action = new MeleeAction(body, offset);
				action.Perform();
				return;
			}
			path = GetPathTo(target.GridPosition);
			GD.Print($"Arno Breker: {path}");
			path.RemoveAt(0);
		}

		if (path.Count > 0) {
			Vector2I destination = (Vector2I) path[0];
			GD.Print(destination);
			if (body.Map_Data.GetBlockingActorAtPosition(destination) != null) {
				return;
			}

			action = new MovementAction(body, destination - body.GridPosition);
			action.Perform();
			path.RemoveAt(0);
		}
	}
}