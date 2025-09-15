using Godot;
using System.Threading.Tasks;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.GUI;

public partial class MessageLog : ScrollContainer
{
	private VBoxContainer MessageList { get; set; }

	private MessageLogData.messageSentEventHandler joinSignal;
	public override void _Ready()
	{
		base._Ready();
		MessageList = GetNode<VBoxContainer>("MessageList");

		foreach (Message msg in MessageLogData.Instance.Messages)
		{
			_ = AddMessageAsync(msg);
		}

		joinSignal = async (Message msg) => await AddMessageAsync(msg);

		MessageLogData.Instance.messageSent += joinSignal;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
		{
			if (joinSignal != null)
			{
				MessageLogData.Instance.messageSent -= joinSignal;
			}
		}
		base._Notification(what);
	}

	private async Task AddMessageAsync(Message message)
	{
		MessageList.AddChild(message);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		EnsureControlVisible(message);
	}
}
