using Godot;

namespace TheLegendOfGustav.GUI;

public partial class Message : Label
{
	private static readonly LabelSettings baseSettings = GD.Load<LabelSettings>("res://assets/definitions/message_label_settings.tres");
	
	private string plainText;
	private int count = 1;

	public Message(string text)
	{
		PlainText = text;
		Text = text;
		LabelSettings = (LabelSettings)baseSettings.Duplicate();
		AutowrapMode = TextServer.AutowrapMode.WordSmart;
	}
	
	public string PlainText 
	{
		get => plainText;
		private set
		{
			plainText = value;
		}
	}
	public int Count
	{
		get => count;
		set
		{
			count = value;
			Text = FullText;
		}
	}

	public string FullText
	{
		get
		{
			if (count > 1)
			{
				return $"{plainText} ({count})";
			}
			return plainText;
		}
	}
}
