using Godot;
using System;
using System.Collections.Generic;
using DemoReplay.Demo;
using DemoReplay.Demo.Stats;

namespace DemoReplay.UI;

public partial class SelectRoundContainer : HBoxContainer
{
	[Export]
	public DemoContainer DemoContainer { get; set; }
	[Export]
	public PackedScene RoundSelectButton { get; set; }

	public override void _Ready()
	{
		DemoContainer.OnStatsLoaded += (sender, statistics) =>
		{
			SetRounds(statistics.Rounds);
		};
	}

	public void SetRounds(List<Round> rounds)
	{
		for (int i = 0; i < rounds.Count; i++)
		{
			AddRoundButton(i, rounds[i]);
		}
	}

	public void AddRoundButton(int roundIndex, Round round)
	{
		var newButton = RoundSelectButton.Instantiate() as RoundSelectButton;
		// newButton.Text = roundNumber.ToString();
		CallDeferred(Node.MethodName.AddChild, newButton);
		newButton.CallDeferred(Button.MethodName.SetText, (roundIndex + 1).ToString());
		newButton.SetDeferred("RoundIndex", roundIndex);
		newButton.SetDeferred("RoundStartTick", round.RoundStartTick);
		newButton.SetDeferred("RoundEndTick", round.RoundEndTick);
		newButton.SetDeferred("WinningTeamNum", (int)round.WinningTeam);
		newButton.RoundSelected += OnRoundSelected;
	}

	public void OnRoundSelected(int roundIndex, int startTick, int endTick)
	{
		DemoContainer.SetRound(roundIndex);
	}
}
