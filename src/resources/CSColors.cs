using DemoFile.Sdk;
using Godot;

namespace DemoReplay.Resources;

public partial class CSColors : Resource
{
	[Export] public Color CTColor { get; set; }
	[Export] public Color TColor { get; set; }
	[Export] public Color NoTeamColor { get; set; }
	[Export] public Color BombCarrierColor { get; set; }
    
	public Color GetColorFromTeam(CSTeamNumber teamNum)
	{
		return teamNum switch
		{
			CSTeamNumber.Terrorist => TColor,
			CSTeamNumber.CounterTerrorist => CTColor,
			_ => NoTeamColor,
		};
	}
	
	public Color GetColorFromTeam(int teamNum)
	{
		return GetColorFromTeam((CSTeamNumber)teamNum);
	}
}