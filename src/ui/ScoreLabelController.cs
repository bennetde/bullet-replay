using DemoFile.Sdk;
using Godot;

namespace DemoReplay.UI;

public partial class ScoreLabelController : HBoxContainer
{
	private Label TScoreLabel { get; set; }
	private Label CTScoreLabel { get; set; }

	public int TScore
	{
		set => TScoreLabel.Text = value.ToString("D2");
	}

	public int CTScore
	{
		set => CTScoreLabel.Text = value.ToString("D2");
	}

	public override void _Ready()
	{
		TScoreLabel = GetNode<Label>("TScoreLabel");
		CTScoreLabel = GetNode<Label>("CTScoreLabel");
	}

	public void SetScore(int csTeamNum, int score)
	{
		switch ((CSTeamNumber)csTeamNum)
		{
			case CSTeamNumber.Terrorist:
				TScore = score;
				break;
			case CSTeamNumber.CounterTerrorist:
				CTScore = score;
				break;
			case CSTeamNumber.Unassigned:
			case CSTeamNumber.Spectator:
			default:
				GD.PushWarning($"Cannot set score for team {((CSTeamNumber)csTeamNum).ToString()}");
				break;
		}
	}

}