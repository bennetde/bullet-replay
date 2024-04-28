using DemoFile.Sdk;

namespace DemoReplay.Demo.Stats;

public class Round
{
	public int RoundStartTick { get; private set; }
	public int RoundEndTick { get; private set; }
	public CSTeamNumber WinningTeam { get; private set; }

	public Round(int roundStartTick, int roundEndTick, CSTeamNumber winningTeam)
	{
		RoundStartTick = roundStartTick;
		RoundEndTick = roundEndTick;
		WinningTeam = winningTeam;
	}
}