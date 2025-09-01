using Godot;

public partial class Message : Label
{
	private static LabelSettings baseSettings = GD.Load<LabelSettings>("res://assets/definitions/message_label_settings.tres");
	private string plainText;
	public string PlainText { get => plainText; }
	private int count = 1;
	public int Count {
		get => count;
		set {
			count = value;
			Text = FullText;
		}
	}

	public string FullText {
		get {
			if (count > 1) {
				return $"{plainText} ({count})";
			}
			return plainText;
		}
	}

	public Message(string text) {
		plainText = text;
		Text = text;
		LabelSettings = (LabelSettings) baseSettings.Duplicate();
		AutowrapMode = TextServer.AutowrapMode.WordSmart;
	}
}
