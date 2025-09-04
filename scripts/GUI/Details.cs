using Godot;
using System;

public partial class Details : CanvasLayer
{
	private static readonly LabelSettings lblSettings = GD.Load<LabelSettings>("res://assets/definitions/message_label_settings.tres");
	private Map map;
	private VBoxContainer entityNames;
	private Godot.Collections.Array<Actor> actors = [];

	private Godot.Collections.Array<Label> actorsLabel = [];

	public override void _Ready()
	{
		base._Ready();
		map = GetParent<Map>();
		entityNames = GetNode<VBoxContainer>("HBoxContainer/PanelContainer/ScrollContainer/Entities");

		SignalBus.Instance.InspectorMoved += OnInspectorWalk;
		SignalBus.Instance.EnterInspectionMode += () => Visible = true;
		SignalBus.Instance.ExitInspectionMode += () => Visible = false;
	}

	public void OnInspectorWalk(Vector2I pos) {
		MapData mapData = map.Map_Data;
		actors = mapData.GetActorsAtPosition(pos);
		UpdateLabels();
	}

	private void UpdateLabels() {
		foreach(Label label in actorsLabel) {
			label.QueueFree();
		}
		actorsLabel.Clear();

		foreach (Actor actor in actors) {
			Label label = new()
			{
				Text = actor.ActorName,
				LabelSettings = lblSettings
			};

			actorsLabel.Add(label);
			entityNames.AddChild(label);
		}
	}
}
 