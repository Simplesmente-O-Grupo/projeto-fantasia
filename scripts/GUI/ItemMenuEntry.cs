using System.Xml;
using Godot;
using TheLegendOfGustav.Entities.Items;

namespace TheLegendOfGustav.GUI;

public partial class ItemMenuEntry : HBoxContainer
{
	private TextureRect icon;
	private Label shortcutLabel;
	private Label nameLabel;
	private Button activateBtn;
	private Button dropBtn;
	private Item item;

	[Signal]
	public delegate void ActivateEventHandler(Item Item);

	[Signal]
	public delegate void DropEventHandler(Item Item);

	public override void _Ready()
	{
		base._Ready();
		icon = GetNode<TextureRect>("Icon");
		shortcutLabel = GetNode<Label>("Shortcut");
		nameLabel = GetNode<Label>("ItemName");
		activateBtn = GetNode<Button>("ActivateBtn");
		dropBtn = GetNode<Button>("DropButton");

		activateBtn.Pressed += () => EmitSignal(SignalName.Activate, item);
		dropBtn.Pressed += () => EmitSignal(SignalName.Drop, item);
	}

	public void Initialize(Item item, char? shortcut)
	{
		this.item = item;
		nameLabel.Text = item.Definition.DisplayName;
		if (shortcut != null)
		{
			shortcutLabel.Text = $"{shortcut}";


			int index = (int)shortcut - 'a';
			InputEventKey activateEvent = new()
			{
				Keycode = Key.A + index
			};

			InputEventKey dropEvent = new()
			{
				Keycode = Key.A + index,
				ShiftPressed = true
			};

			Shortcut shortie = new()
			{
				Events = [activateEvent]
			};

			Shortcut dropperino = new()
			{
				Events = [dropEvent]
			};

			activateBtn.Shortcut = shortie;
			dropBtn.Shortcut = dropperino;
		}
		else
		{
			shortcutLabel.Text = "";
		}
		icon.Texture = item.Definition.Icon;
	}
}
