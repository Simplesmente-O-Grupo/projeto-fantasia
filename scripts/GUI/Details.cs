using Godot;
using TheLegendOfGustav.Entities;
using TheLegendOfGustav.Map;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.GUI;

public partial class Details : CanvasLayer
{
	private static readonly LabelSettings lblSettings = GD.Load<LabelSettings>("res://assets/definitions/message_label_settings.tres");

	[Export]
	private Map.Map map;
	private VBoxContainer EntityNames { get; set; }
	private Godot.Collections.Array<Entity> Entities { get; set; } = [];

	private Godot.Collections.Array<Label> ActorsLabel { get; set; } = [];

	private SignalBus.EnterInspectionModeEventHandler enterLambda;
	private SignalBus.ExitInspectionModeEventHandler exitLambda;

	public override void _Ready()
	{
		base._Ready();
		EntityNames = GetNode<VBoxContainer>("HBoxContainer/PanelContainer/ScrollContainer/Entities");

		enterLambda = () => Visible = true;
		exitLambda = () => Visible = false;
		SignalBus.Instance.InspectorMoved += OnInspectorWalk;
		SignalBus.Instance.EnterInspectionMode += enterLambda;
		SignalBus.Instance.ExitInspectionMode += exitLambda;
	}

	public void OnInspectorWalk(Vector2I pos)
	{
		MapData mapData = map.MapData;
		Entities = mapData.GetEntitiesAtPosition(pos);
		UpdateLabels();
	}

	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
		{
			SignalBus.Instance.InspectorMoved -= OnInspectorWalk;
			if (enterLambda != null)
			{
				SignalBus.Instance.EnterInspectionMode -= enterLambda;
			}
			if (exitLambda != null)
			{
				SignalBus.Instance.ExitInspectionMode -= exitLambda;
			}
		}
		base._Notification(what);
	}

	private void UpdateLabels()
	{
		foreach (Label label in ActorsLabel)
		{
			label.QueueFree();
		}

		ActorsLabel.Clear();

		foreach (Entity entity in Entities)
		{
			Label label = new()
			{
				Text = entity.DisplayName,
				LabelSettings = lblSettings
			};

			ActorsLabel.Add(label);
			EntityNames.AddChild(label);
		}
	}
}
