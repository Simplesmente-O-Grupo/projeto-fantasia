using Godot;
using TheLegendOfGustav.Entities.Actors;

namespace TheLegendOfGustav.Magic;

[GlobalClass]
public abstract partial class SpellEffect : Resource
{
	public abstract void Apply(Actor caster, Actor target);
}
