using Godot;
using System;
using DemoFile.Sdk;

public partial class RoundTimerUI : Control
{
	private Label _roundTimeLabel;
	private Label _scoreLabel;

	private int _team1Score;
	private int _team2Score;
	public override void _Ready()
	{
		_roundTimeLabel = GetNode<Label>("Panel/RoundTimeLabel");
		_scoreLabel = GetNode<Label>("Panel/ScoreLabel");
	}

	public void SetTime(float time)
	{
		if (time >= 0.0f)
		{
			var minutes = Mathf.FloorToInt(time / 60.0);
			var seconds = Mathf.Floor(time - minutes * 60.0f);
			_roundTimeLabel.Text = String.Format("{0:00}:{1:00}",minutes, seconds);
		} else 
			_roundTimeLabel.Text = time.ToString();
	}

	public void SetScore(int teamNum, int score)
	{
		var team = (CSTeamNumber)teamNum;
		if (team == CSTeamNumber.CounterTerrorist)
		{
			_team1Score = score;
		}
		else
		{
			_team2Score = score;
		}

		_scoreLabel.Text = $"{_team1Score} - {_team2Score}";
	}
}
