using Godot;
using TheLegendOfGustav.Entities;
using TheLegendOfGustav.Map;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.GUI;

public partial class Details : CanvasLayer
{
	private static readonly LabelSettings lblSettings = GD.Load<LabelSettings>("res://assets/definitions/message_label_settings.tres");
	
	private Map.Map Map { get; set; }
	private VBoxContainer EntityNames { get; set; }
	private Godot.Collections.Array<Entity> Entities { get; set; } = [];

	private Godot.Collections.Array<Label> ActorsLabel { get; set; } = [];

	public override void _Ready()
	{
		base._Ready();
		Map = GetParent<Map.Map>();
		EntityNames = GetNode<VBoxContainer>("HBoxContainer/PanelContainer/ScrollContainer/Entities");

		SignalBus.Instance.InspectorMoved += OnInspectorWalk;
		SignalBus.Instance.EnterInspectionMode += () => Visible = true;
		SignalBus.Instance.ExitInspectionMode += () => Visible = false;
	}

	public void OnInspectorWalk(Vector2I pos)
	{
		MapData mapData = Map.MapData;
		Entities = mapData.GetEntitiesAtPosition(pos);
		UpdateLabels();
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
