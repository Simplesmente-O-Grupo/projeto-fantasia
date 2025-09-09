using Godot;

public partial class ItemMenuEntry : HBoxContainer
{
	private TextureRect icon;
	private Label shortcutLabel;
	private Label nameLabel;
	private Button activateBtn;
	private Button dropBtn;

	[Signal]
	public delegate void ActivateEventHandler(ConsumableItem Item);

	[Signal]
	public delegate void DropEventHandler(ConsumableItem item);

	private ConsumableItem item;

	public void Initialize(ConsumableItem item, char? shortcut) {
		this.item = item;
		nameLabel.Text = item.DisplayName;
		if (shortcut != null) {
			shortcutLabel.Text = $"{shortcut}";
		} else {
			shortcutLabel.Text = "";
		}
		icon.Texture = item.Texture;
	}

	public override void _Ready() {
		base._Ready();
		icon = GetNode<TextureRect>("Icon");
		shortcutLabel = GetNode<Label>("Shortcut");
		nameLabel = GetNode<Label>("ItemName");
		activateBtn = GetNode<Button>("ActivateBtn");
		dropBtn = GetNode<Button>("DropButton");

		activateBtn.Pressed += () => EmitSignal(SignalName.Activate, item);
		dropBtn.Pressed += () => EmitSignal(SignalName.Drop, item);
	}
}
