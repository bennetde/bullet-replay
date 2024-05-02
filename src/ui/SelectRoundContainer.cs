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
		int roundNumber = 1;
		for (int i = 0; i < rounds.Count; i++)
		{
			if (rounds[i].IsKnifeRound)
			{
				roundNumber--;
			}
			AddRoundButton(i, roundNumber, rounds[i]);
			roundNumber++;
		}
	}

	public void AddRoundButton(int roundIndex, int roundNumber, Round round)
	{
		var newButton = RoundSelectButton.Instantiate() as RoundSelectButton;
		// newButton.Text = roundNumber.ToString();
		CallDeferred(Node.MethodName.AddChild, newButton);
		newButton.CallDeferred(global::RoundSelectButton.MethodName.SetText, (roundNumber).ToString());
		newButton.SetDeferred("RoundIndex", roundIndex);
		newButton.SetDeferred("RoundStartTick", round.RoundStartTick);
		newButton.SetDeferred("RoundEndTick", round.RoundEndTick);
		newButton.SetDeferred("WinningTeamNum", (int)round.WinningTeam);
		newButton.SetDeferred("IsKnifeRound", round.IsKnifeRound);
		newButton.RoundSelected += OnRoundSelected;
	}

	public void OnRoundSelected(int roundIndex, int startTick, int endTick)
	{
		DemoContainer.SetRound(roundIndex);
	}
}
