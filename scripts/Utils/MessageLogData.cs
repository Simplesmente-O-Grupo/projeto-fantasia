using Godot;
using TheLegendOfGustav.GUI;

namespace TheLegendOfGustav.Utils;

public partial class MessageLogData : Node
{
	/// <summary>
	/// Acionado sempre que uma mensagem for adicionada para o log.
	/// </summary>
	/// <param name="text">Mensagem.</param>
	[Signal]
	public delegate void messageSentEventHandler(Message message);

	public static MessageLogData Instance { get; private set; }

	public Godot.Collections.Array<Message> Messages { get; private set; } = [];

	private Message LastMessage
	{
		get
		{
			if (Messages.Count <= 0)
			{
				return null;
			}
			return Messages[^1];
		}
	}

	public override void _Ready()
	{
		base._Ready();
		Instance = this;
	}

	public void ClearMessages()
	{
		for (int i = Messages.Count - 1; i >= 0; i--)
		{
			Message message = Messages[i];
			Messages.RemoveAt(i);
			message?.QueueFree();
		}
	}

	public void AddMessage(string text)
	{
		if (LastMessage != null && LastMessage.PlainText == text)
		{
			LastMessage.Count++;
			return;
		}

		Message message = new(text);
		Messages.Add(message);
		EmitSignal(SignalName.messageSent, message);
	}
}