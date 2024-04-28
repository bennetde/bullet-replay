using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using DemoFile.Sdk;
using DemoReplay.Demo;
using DemoReplay.UI;

public partial class MatchInfoPanel : Panel
{
	
	[Export]
	public DemoContainer DemoContainer { get; set; }
	
	[Export]
	public PackedScene PlayerCard { get; set; }
	[Export]
	private VBoxContainer CTsList { get; set; }
	[Export]
	private VBoxContainer TsList { get; set; }
	
	[Export]
	private ScoreLabelController ScoreLabelController { get; set; }

	private Dictionary<uint, PlayerCard> Players { get; set; }


	public override void _Ready()
	{
		Players = new();

		DemoContainer.PlayerControllerCreatedEventHandler += OnPlayerCreate;
		DemoContainer.PlayerControllerDeletedEventHandler += OnPlayerDelete;
		DemoContainer.PlayerControllerHealthChangedEventHandler += OnPlayerHealthChanged;
		DemoContainer.ScoreChangedEventHandler += OnScoreChanged;
		DemoContainer.PlayerControllerMoneyChangedEventHandler += OnPlayerMoneyChanged;
		DemoContainer.PlayerControllerTeamChangedEventHandler += OnPlayerTeamChanged;
		DemoContainer.PlayerKillsUpdatedEventHandler += OnPlayerKill;
		DemoContainer.PlayerDeathsUpdatedEventHandler += OnPlayerDeath;
		DemoContainer.PlayerAssistsUpdatedEventHandler += OnPlayerAssist;
		DemoContainer.PlayerAddItemEventHandler += OnPlayerAddItem;
		DemoContainer.PlayerSetItemsEventHandler += OnPlayerSetItems;
	}

	private void OnPlayerAddItem(object sender, (CCSPlayerController, string) e)
	{
		// if (Players.TryGetValue(e.Item1.EntityIndex.Value, out var card))
		// {
		// 	card.CallDeferred("AddItem", e.Item2);
		// }
		// else
		// {
		// 	GD.PushWarning($"Couldn't find {e.Item1.PlayerName} in OnPlayerAddItem");
		// }
	}

	private void OnPlayerSetItems(object sender, (CCSPlayerController, IEnumerable<CCSWeaponBase>) e)
	{
		var weapons = e.Item2;
		if (Players.TryGetValue(e.Item1.EntityIndex.Value, out var card))
		{
			card.CallDeferred(global::PlayerCard.MethodName.DisableWeaponTextures);
			// attackerCard.CallDeferred("AddItem");
			foreach (var weapon in weapons)
			{
				card.CallDeferred("AddItem", weapon.EconItem.Name);
			}
		}
		else
		{
			GD.PushWarning($"Couldn't find {e.Item1.PlayerName} in OnPlayerSetItems");
		}
	}

	public void Clear()
	{
		foreach (var pair in Players)
		{
			pair.Value.CallDeferred(Node.MethodName.QueueFree);
			Players.Remove(pair.Key);
		}
	}

	public void SetPlayers(IEnumerable<CCSPlayerController> controllers)
	{
		foreach (var controller in controllers)
		{
			if (controller.PlayerPawn == null)
			{
				continue;
			}

			if (controller.PlayerName.Equals(""))
			{
				GD.PushWarning("Controller has no name");
				continue;
			}

			OnPlayerCreate(null, controller);
		}
	}

	private void OnPlayerKill(object sender, CCSPlayerController e)
	{
		if (e.PlayerPawn == null) return;
		if (Players.TryGetValue(e.EntityIndex.Value, out var attackerCard))
		{
			attackerCard.SetDeferred("Kills", e.ActionTrackingServices.MatchStats.Kills);
		}
	}

	private void OnPlayerDeath(object sender, CCSPlayerController e)
	{
		if (e.PlayerPawn == null) return;
		if (Players.TryGetValue(e.EntityIndex.Value, out var attackerCard))
		{
			attackerCard.SetDeferred("Deaths", e.ActionTrackingServices.MatchStats.Deaths);
		}
	}
	
	private void OnPlayerAssist(object sender, CCSPlayerController e)
	{
		if (e.PlayerPawn == null) return;
		if (Players.TryGetValue(e.EntityIndex.Value, out var attackerCard))
		{
			attackerCard.SetDeferred("Assists", e.ActionTrackingServices.MatchStats.Assists);
		}
	}

	private void OnPlayerTeamChanged(object sender, (CCSPlayerController, CSTeamNumber, CSTeamNumber) values)
	{
		var pawn = values.Item1;
		var oldTeam = values.Item2;
		var newTeam = values.Item3;
		CallDeferred(MethodName.SwapTeamList, pawn.EntityIndex.Value, (int)oldTeam, (int)newTeam);
	}

	private void SwapTeamList(uint entityIndex, int oldTeamNum, int newTeamNum)
	{
		var oldList = (CSTeamNumber)oldTeamNum == CSTeamNumber.Terrorist ? TsList : CTsList;
		var newList = (CSTeamNumber)newTeamNum == CSTeamNumber.Terrorist ? TsList : CTsList;
		if (Players.TryGetValue(entityIndex, out var card))
		{
			oldList.RemoveChild(card);
			newList.AddChild(card);
			
			// card.TeamNum = (int)newTeam;
			card.SetDeferred("TeamNum", newTeamNum);
		}
		else
		{
			GD.PushWarning("Couldn't find player changing teams in old team's dictionary.");
		}
	}

	private void OnPlayerCreate(object sender, CCSPlayerController ctrl)
	{
		// if (pair.Item1.Controller == null) return;
		CallDeferred(MethodName.AddPlayer, ctrl.EntityIndex.Value, ctrl.PlayerName, (int)ctrl.CSTeamNum);
	}

	private void OnPlayerDelete(object sender, CCSPlayerController e)
	{
		if (Players.TryGetValue(e.EntityIndex.Value, out var card))
		{
			Players.Remove(e.EntityIndex.Value);
            card.QueueFree();
		}
		else
		{
			GD.PushWarning($"Didn't find {e.EntityIndex.Value}/{e.PlayerName} in OnPlayerDelete");
		}
	}

	private void OnPlayerHealthChanged(object sender, (CCSPlayerController, int) pair)
	{
		var ctrl = pair.Item1;
		var newHealth = pair.Item2;
		if (Players.TryGetValue(ctrl.EntityIndex.Value, out var card))
		{
			card.SetDeferred("Health", newHealth);
		}
		else
		{
			GD.PushWarning($"Didn't find {ctrl.EntityIndex.Value}/{ctrl.PlayerName} in OnPlayerHealthChanged");
		}
	}

	private void OnScoreChanged(object sender, (CSTeamNumber, int) values)
	{
		ScoreLabelController.CallDeferred(ScoreLabelController.MethodName.SetScore, (int)values.Item1, values.Item2);
	}
	
	private void OnPlayerMoneyChanged(object sender, (CCSPlayerController, int) values)
	{
		if (Players.TryGetValue(values.Item1.EntityIndex.Value, out var card))
		{
			card.SetDeferred("Money", values.Item2);
		}
		else
		{
			GD.PushWarning($"Didn't find {values.Item1.PlayerName} in OnPlayerMoneyChanged");
		}
	}

	public void AddPlayer(uint entityIndex, string name, int teamIndex)
	{
		var newCard = PlayerCard.Instantiate() as PlayerCard;
		if (newCard == null) throw new NullReferenceException("Couldn't instantiate card");
		switch ((CSTeamNumber)teamIndex)
		{
			case CSTeamNumber.Terrorist:
				Players.Add(entityIndex, newCard);
				TsList.CallDeferred(MethodName.AddChild, newCard);
				break;
			case CSTeamNumber.CounterTerrorist:
				Players.Add(entityIndex, newCard);
				CTsList.CallDeferred(MethodName.AddChild, newCard);
				break;
			default:
				GD.PushWarning($"Tried adding player {name} with no team");
				return;
		}

		newCard.PlayerName = name;
		newCard.TeamNum = teamIndex;
	}
}
