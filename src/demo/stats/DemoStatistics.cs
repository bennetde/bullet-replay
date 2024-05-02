using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DemoFile;
using DemoFile.Sdk;
using Godot;

namespace DemoReplay.Demo.Stats;

public class DemoStatistics
{
	public List<Round> Rounds { get; private set; }
	public bool DemoContainsKnifeRound { get; private set; }

	private DemoParser Demo { get; set; }
	
	private int LatestRoundStartTick { get; set; }
	private bool LatestRoundIsKnife { get; set; }
	
	private DemoStatistics()
	{
		Rounds = new List<Round>();
		Demo = new DemoParser();
		DemoContainsKnifeRound = false;
		
		Demo.Source1GameEvents.RoundStart += OnRoundStart;
		Demo.Source1GameEvents.RoundEnd += OnRoundEnd;
	}
	
	private void OnRoundStart(Source1RoundStartEvent e)
	{
		LatestRoundStartTick = Demo.CurrentDemoTick.Value;
		LatestRoundIsKnife = false;
		// if (Rounds.Count != 0) return;
	}

	private void OnRoundEnd(Source1RoundEndEvent e)
	{
		// HACK: Find out a proper way to detect knife rounds. RoundStartEvent is not called on the first round in some cases.
		// Only first round should be a Knife Round
		bool isKnife = Demo.FileHeader.ServerName.Contains("FACEIT") && Rounds.Count == 0;
		
		var roundEndTick = Demo.CurrentDemoTick.Value;
		var winningTeam = (CSTeamNumber)e.Winner;
		
		// Move the start tick for the first round forward a bit, so that PlayerControllers aren't created very time the user skips to the first round.
		if (LatestRoundStartTick == 0)
		{
			LatestRoundStartTick = 100;
		}
		
		Rounds.Add(new Round(LatestRoundStartTick, roundEndTick, winningTeam, isKnife));
	}

	public static async Task<DemoStatistics> CreateDemoStatistics(string demoPath)
	{
		var stats = new DemoStatistics();
		await stats.Demo.ReadAllAsync(File.OpenRead(demoPath), default);
		return stats;
	}
}