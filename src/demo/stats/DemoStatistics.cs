using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DemoFile;
using DemoFile.Sdk;
using Godot;

namespace DemoReplay.Demo.Stats;

public class DemoStatistics
{
	public List<Round> Rounds { get; private set; }

	private DemoParser Demo { get; set; }
	
	private int LatestRoundStartTick { get; set; }
	
	private DemoStatistics()
	{
		Rounds = new List<Round>();
		Demo = new DemoParser();
		// Demo.Source1GameEvents.RoundPoststart += OnRoundStart;
		Demo.Source1GameEvents.RoundStart += OnRoundStart;
		Demo.Source1GameEvents.RoundEnd += OnRoundEnd;
	}
	
	private void OnRoundStart(Source1RoundStartEvent e)
	{
		LatestRoundStartTick = Demo.CurrentDemoTick.Value;
	}

	private void OnRoundEnd(Source1RoundEndEvent e)
	{
		var roundEndTick = Demo.CurrentDemoTick.Value;
		var winningTeam = (CSTeamNumber)e.Winner;
		GD.Print($"Round {Rounds.Count + 1} starts at {LatestRoundStartTick} and ends at {roundEndTick}");
		Rounds.Add(new Round(LatestRoundStartTick, roundEndTick, winningTeam));
	}

	public static async Task<DemoStatistics> CreateDemoStatistics(string demoPath)
	{
		var stats = new DemoStatistics();
		await stats.Demo.ReadAllAsync(File.OpenRead(demoPath), default);
		return stats;
	}
}