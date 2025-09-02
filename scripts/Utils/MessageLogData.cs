using Godot;

public partial class MessageLogData : Node
{
	public static MessageLogData Instance { get; private set; }
	private Godot.Collections.Array<Message> messages = [];
	public Godot.Collections.Array<Message> Messages {get => messages;}

private Message LastMessage {
		get {
			if (messages.Count <= 0) {
				return null;
			}
			return messages[^1];
		}
	}

	public void ClearMessages() {
		for (int i = messages.Count - 1; i >= 0; i--) {
			Message message = messages[i];
			messages.RemoveAt(i);
			message.QueueFree();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		Instance = this;
	}

	public void AddMessage(string text) {
		if (LastMessage != null && LastMessage.PlainText == text) {
			LastMessage.Count++;
			return;
		}

		Message message = new(text);
		messages.Add(message);
		EmitSignal(SignalName.messageSent, message);
	}
	
	/// <summary>
	/// Acionado sempre que uma mensagem for adicionada para o log.
	/// </summary>
	/// <param name="text">Mensagem.</param>
	[Signal]
	public delegate void messageSentEventHandler(Message message);

}