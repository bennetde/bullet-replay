using Godot;
using System;
using DemoReplay.Resources;

public partial class RoundSelectButton : Button
{
	[Signal]
	public delegate void RoundSelectedEventHandler(int roundIndex, int roundStartTick, int roundEndTick);
	[Export]
	public CSColors Colors { get; set; }
	
	public int RoundStartTick { get; set; }
	public int RoundEndTick { get; set; }
	
	public int RoundIndex { get; set; }

	public int WinningTeamNum
	{
		set => SelfModulate = Colors.GetColorFromTeam(value);
	}

	public override void _Ready()
	{
		Pressed += () =>
		{
			EmitSignal(SignalName.RoundSelected, RoundIndex, RoundStartTick, RoundEndTick);
		};
	}
}
