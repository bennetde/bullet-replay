using Godot;
using System;
using DemoReplay.Resources;

public partial class RoundSelectButton : Panel
{
	[Signal]
	public delegate void RoundSelectedEventHandler(int roundIndex, int roundStartTick, int roundEndTick);
	[Export]
	public CSColors Colors { get; set; }
	
	public int RoundStartTick { get; set; }
	public int RoundEndTick { get; set; }
	
	public int RoundIndex { get; set; }
	
	public bool IsKnifeRound
	{
		get
		{
			return Button.Text == "K";
		}
		set
		{
			if (value)
			{
				GD.Print(value.ToString());
				Button.Text = "K";
			}
		}
	}

	public int WinningTeamNum
	{
		set => SelfModulate = Colors.GetColorFromTeam(value);
	}
	
	private Button Button { get; set; }

	public override void _Ready()
	{
		Button = GetNode<Button>("RoundButton");
		Button.Pressed += () =>
		{
			EmitSignal(SignalName.RoundSelected, RoundIndex, RoundStartTick, RoundEndTick);
		};
	}

	public void SetText(string text)
	{
		Button.Text = text;
	}
}
