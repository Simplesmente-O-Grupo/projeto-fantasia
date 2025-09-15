using Godot;
using TheLegendOfGustav.Entities.Actors;
using TheLegendOfGustav.Entities.Actions;
using TheLegendOfGustav.Utils;

using TheLegendOfGustav.InputHandling;
using TheLegendOfGustav.Map;
using TheLegendOfGustav.Magic;

namespace TheLengendOfGustav.InputHandling;

/// <summary>
/// Esquema de controles para quando o jogador precisar escolher um alvo
/// de feitiço.
/// 
/// É bem similar ao InspectorInputHandler.
/// </summary>
public partial class CastSpellInputHandler : BaseInputHandler
{
	private static readonly PackedScene inspectorScene = GD.Load<PackedScene>("res://scenes/Inspector.tscn");

	private static readonly Godot.Collections.Dictionary<string, Vector2I> directions = new()
	{
		{"walk-up", Vector2I.Up},
		{"walk-down", Vector2I.Down},
		{"walk-left", Vector2I.Left},
		{"walk-right", Vector2I.Right},
		{"walk-up-right", Vector2I.Up + Vector2I.Right},
		{"walk-up-left", Vector2I.Up + Vector2I.Left},
		{"walk-down-right", Vector2I.Down + Vector2I.Right},
		{"walk-down-left", Vector2I.Down + Vector2I.Left},
	};

	private Inspector inspector = null;
	private SpellResource selectedSpell = null;
	[Export]
	private Map map;

	SignalBus.PlayerSpellChooseLocationEventHandler spellLocationLambda;

	public override void _Ready()
	{
		base._Ready();

		spellLocationLambda = (SpellResource spell) => selectedSpell = spell;
		// O jogador informa qual feitiço será usado.
		SignalBus.Instance.PlayerSpellChooseLocation += spellLocationLambda;
	}

	public override void Enter()
	{
		inspector = inspectorScene.Instantiate<Inspector>();

		inspector.GridPosition = map.MapData.Player.GridPosition;

		map.AddChild(inspector);
	}

	public override void Exit()
	{
		selectedSpell = null;
		inspector.QueueFree();
	}


	public override Action GetAction(Player player)
	{
		Action action = null;

		foreach (var direction in directions)
		{
			if (Input.IsActionJustPressed(direction.Key))
			{
				inspector.Walk(direction.Value);
			}
		}

		if (selectedSpell != null && Input.IsActionJustPressed("ui_accept"))
		{
			action = new SpellAction(player, inspector.GridPosition - player.GridPosition, selectedSpell);

			GetParent<InputHandler>().SetInputHandler(InputHandlers.MainGame);
		}

		// Se o jogador cancelar a seleção,
		// Mandamos um sinal avisando que o feitiço não foi executado com sucesso.
		// Pergaminhos usam esta informação.
		if (Input.IsActionJustPressed("quit"))
		{
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.PlayerSpellCast, false);
			GetParent<InputHandler>().SetInputHandler(InputHandlers.MainGame);
		}

		return action;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
		{
			if (spellLocationLambda != null)
			{
				SignalBus.Instance.PlayerSpellChooseLocation -= spellLocationLambda;
			}
		}
		base._Notification(what);
	}
}