using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Utils;

namespace TheLegendOfGustav.Entities.Actions;

public partial class EscapeAction(Actor actor) : Action(actor)
{
	public override bool Perform()
	{
		Actor.MapData.SaveGame();
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.EscapeRequested);
		return false;
	}
}