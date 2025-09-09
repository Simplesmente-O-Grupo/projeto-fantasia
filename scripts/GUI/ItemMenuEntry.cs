using Godot;
using TheLegendOfGustav.Entities.Items;

namespace TheLegendOfGustav.GUI;

public partial class ItemMenuEntry : HBoxContainer
{
	[Signal]
	public delegate void ActivateEventHandler(ConsumableItem Item);

	[Signal]
	public delegate void DropEventHandler(ConsumableItem Item);

	private TextureRect Icon { get; set; }
	private Label ShortcutLabel { get; set; }
	private Label NameLabel { get; set; }
	private Button ActivateBtn { get; set; }
	private Button DropBtn { get; set; }
	private ConsumableItem Item { get; set; }

	public override void _Ready()
	{
		base._Ready();
		Icon = GetNode<TextureRect>("Icon");
		ShortcutLabel = GetNode<Label>("Shortcut");
		NameLabel = GetNode<Label>("ItemName");
		ActivateBtn = GetNode<Button>("ActivateBtn");
		DropBtn = GetNode<Button>("DropButton");

		ActivateBtn.Pressed += () => EmitSignal(SignalName.Activate, Item);
		DropBtn.Pressed += () => EmitSignal(SignalName.Drop, Item);
	}

	public void Initialize(ConsumableItem item, char? shortcut)
	{
		Item = item;
		NameLabel.Text = item.DisplayName;
		if (shortcut != null)
		{
			ShortcutLabel.Text = $"{shortcut}";
		}
		else
		{
			ShortcutLabel.Text = "";
		}
		Icon.Texture = item.Texture;
	}
}
