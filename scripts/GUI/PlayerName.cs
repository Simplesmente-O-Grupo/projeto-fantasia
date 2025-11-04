using Godot;

namespace TheLegendOfGustav.GUI;
public partial class PlayerName : Control
{

	private LineEdit nameEdit;
	private Button startButton;
	[Signal]
	public delegate void NewGameRequestEventHandler(string name);

	public override void _Ready()
	{
		nameEdit = GetNode<LineEdit>("VBoxContainer/thename");
		startButton = GetNode<Button>("VBoxContainer/Button");
		startButton.Pressed += OnClick;
	}

	private void OnClick()
	{
		string name = nameEdit.Text;
		EmitSignal(SignalName.NewGameRequest, name);
	}
}
