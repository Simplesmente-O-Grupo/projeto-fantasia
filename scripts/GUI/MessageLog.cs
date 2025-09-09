using Godot;
using System.Threading.Tasks;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.GUI;

public partial class MessageLog : ScrollContainer
{
	private VBoxContainer MessageList { get; set; }

	public override void _Ready()
	{
		base._Ready();
		MessageList = GetNode<VBoxContainer>("MessageList");

		foreach (Message msg in MessageLogData.Instance.Messages)
		{
			_ = AddMessageAsync(msg);
		}

		MessageLogData.Instance.messageSent += async (Message msg) => await AddMessageAsync(msg);
	}

	private async Task AddMessageAsync(Message message)
	{
		MessageList.AddChild(message);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		EnsureControlVisible(message);
	}
}
