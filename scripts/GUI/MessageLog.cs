using Godot;
using System;
using System.Threading.Tasks;

public partial class MessageLog : ScrollContainer
{
	private Message lastMessage = null;
	private VBoxContainer MessageList;

	public override void _Ready()
	{
		base._Ready();
		MessageList = GetNode<VBoxContainer>("MessageList");
		SignalBus.Instance.messageSent += async (string text) => await AddMessageAsync(text);
	}

	private async Task AddMessageAsync(string text) {
		if (lastMessage != null && lastMessage.PlainText == text) {
			lastMessage.Count++;
			return;
		}

		Message message = new(text);
		lastMessage = message;
		MessageList.AddChild(message);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		EnsureControlVisible(message);
	}

	/// <summary>
    /// Método estático disponível em todo o escopo do jogo.
    /// </summary>
    /// <param name="text"></param>
	public static void SendMessage(string text) {
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.messageSent, text);
	}
}
