using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Actions;

public partial class TakeStairsAction : Action
{
	public TakeStairsAction(Actor actor) : base(actor)
	{
		cost = 0;
	}

	public override bool Perform()
	{
		if (Actor.GridPosition == MapData.DownstairsLocation)
		{
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.PlayerDescent);
			MessageLogData.Instance.AddMessage("Você desce as escadas...");
			return true;
		}
		else
		{
			MessageLogData.Instance.AddMessage("Não tem escadas aqui...");
			return false;
		}
	}
}