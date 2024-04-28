using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoFile.Sdk;
using Godot;

namespace DemoReplay.Demo;

public partial class DemoContainer
{
	public event EventHandler<CCSPlayerController> PlayerControllerCreatedEventHandler;
	public event EventHandler<CCSPlayerController> PlayerControllerDeletedEventHandler;
	public event EventHandler<(CCSPlayerController, Vector2)> PlayerControllerMovedEventHandler;
	public event EventHandler<(CCSPlayerController, int)> PlayerControllerHealthChangedEventHandler; 
	public event EventHandler<(CCSPlayerController, float)> PlayerControllerRotatedEventHandler; 
	public event EventHandler<(CCSPlayerController, int)> PlayerControllerMoneyChangedEventHandler; 
	public event EventHandler<CCSPlayerController> PlayerKillsUpdatedEventHandler;
	public event EventHandler<CCSPlayerController> PlayerDeathsUpdatedEventHandler;
	public event EventHandler<CCSPlayerController> PlayerAssistsUpdatedEventHandler;
	public event EventHandler<(CCSPlayerController, string)> PlayerAddItemEventHandler;
	public event EventHandler<(CCSPlayerController, IEnumerable<CCSWeaponBase>)> PlayerSetItemsEventHandler;
	public event EventHandler<(CCSPlayerController, CSTeamNumber, CSTeamNumber)> PlayerControllerTeamChangedEventHandler; 

	private void InitializePlayerEventHandlers()
	{
		Demo.EntityEvents.CCSPlayerController.Create += controller =>
		{
			if (WannaSeek) return;
			PlayerControllerCreatedEventHandler?.Invoke(this, controller);
			if (controller.PlayerPawn == null)
			{
				return;
			}
			PlayerSetItemsEventHandler?.Invoke(this, (controller, controller.PlayerPawn.Weapons));
		};
		
		Demo.EntityEvents.CCSPlayerController.Delete += controller =>
		{
			if (WannaSeek) return;
			PlayerControllerDeletedEventHandler?.Invoke(this, controller);
		};

		Demo.EntityEvents.CCSPlayerController.AddChangeCallback(ctrl => ctrl.PlayerPawn?.Health,
			(controller, _, h) =>
			{
				PlayerControllerHealthChangedEventHandler?.Invoke(this, (controller, (int)h));
			}
		);
		
		Demo.EntityEvents.CCSPlayerController.AddChangeCallback(ctrl => ctrl.PlayerPawn?.Origin,
			(controller, _, h) =>
			{
				PlayerControllerMovedEventHandler?.Invoke(this, (controller, new Vector2(h.Value.X, h.Value.Y)));
			}
		);
		
		Demo.EntityEvents.CCSPlayerController.AddChangeCallback(
			ctrl => ctrl.PlayerPawn?.EyeAngles,
			(ctrl, _, angle) =>
			{
				PlayerControllerRotatedEventHandler?.Invoke(this, (ctrl, angle.Value.Yaw));
			}
		);

		Demo.EntityEvents.CCSPlayerController.AddChangeCallback(ctrl => ctrl.ActionTrackingServices?.MatchStats.Kills,
			(controller, _, _) =>
			{
				if (WannaSeek) return;
				PlayerKillsUpdatedEventHandler?.Invoke(this, controller);
			}
		);
		
		Demo.EntityEvents.CCSPlayerController.AddChangeCallback(ctrl => ctrl.ActionTrackingServices?.MatchStats.Deaths,
			(controller, _, _) =>
			{
				if (WannaSeek) return;
				PlayerDeathsUpdatedEventHandler?.Invoke(this, controller);
			}
		);
		
		Demo.EntityEvents.CCSPlayerController.AddChangeCallback(ctrl => ctrl.ActionTrackingServices?.MatchStats.Assists,
			(controller, _, _) =>
			{
				if (WannaSeek) return;
				PlayerAssistsUpdatedEventHandler?.Invoke(this, controller);
			}
		);
		
		Demo.EntityEvents.CCSPlayerController.AddChangeCallback(ctrl => ctrl.CSTeamNum,
			(controller, oldTeam, newTeam) =>
			{
				PlayerControllerTeamChangedEventHandler?.Invoke(this, (controller, oldTeam, newTeam));
			}
		);
		
		Demo.EntityEvents.CCSPlayerController.AddChangeCallback(controller => controller.InGameMoneyServices?.Account,
			(controller, _, newMoney) =>
			{
				PlayerControllerMoneyChangedEventHandler?.Invoke(this, (controller, (int)newMoney));
			}
		);

		// Demo.EntityEvents.CCSPlayerController.AddChangeCallback(ctrl => ctrl.Pawn.,
		// 	(ctrl, _, newInv) =>
		// 	{
		// 		
		// 	}	
		// );

		Demo.Source1GameEvents.ItemPickup += e =>
		{
			PlayerAddItemEventHandler?.Invoke(this, (e.Player, e.Item));
		};

		Demo.Source1GameEvents.ItemRemove += e =>
		{
			// GD.Print($"{e.Player.PlayerName} removed up {e.Item}");
		};
		
		Demo.EntityEvents.CCSPlayerPawn.AddCollectionChangeCallback(pawn => pawn.Weapons,
			(pawn, _, newInv) =>
			{
				var newWeaponsList = newInv.ToList();
				var controller = pawn.Controller;
				if (controller == null) return;
				PlayerSetItemsEventHandler?.Invoke(this, (controller, newWeaponsList));
			}
		);

	}
	
}