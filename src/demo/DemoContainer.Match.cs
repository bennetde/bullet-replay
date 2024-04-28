using System;
using DemoFile.Sdk;
using Godot;

namespace DemoReplay.Demo;

public partial class DemoContainer
{
	public EventHandler<string> MapLoadedEventHandler;
	public EventHandler<(CSTeamNumber, int)> ScoreChangedEventHandler;

	public void InitializeMatchEventHandlers()
	{
		Demo.EntityEvents.CCSTeam.AddChangeCallback(team => team.Score,
			(team, _, newScore) =>
			{
				ScoreChangedEventHandler?.Invoke(this, (team.CSTeamNum, newScore));
			}
		);
	}
}