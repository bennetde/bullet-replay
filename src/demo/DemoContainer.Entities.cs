using System;
using DemoFile.Sdk;
using Godot;

namespace DemoReplay.Demo;

public struct SmokeGrenadeMovedEventArgs
{
    public CCSPlayerPawn pawn;
    public Vector2 newPos;
}

public partial class DemoContainer
{
	public event EventHandler<(CCSPlayerPawn, string)> PlayerPawnCreatedEventHandler;
	public event EventHandler<CCSPlayerPawn> PlayerPawnDeletedEventHandler;
	public event EventHandler<uint> PlayerDeathEventHandler;
	public event EventHandler<uint> PlayerRespawnEventHandler;
	public event EventHandler<CCSPlayerPawn> PlayerRotatedEventHandler;
	public event EventHandler<uint> PlayerShootEventHandler;
	public event EventHandler<CCSPlayerPawn> PlayerHealthChangedEventHandler;
    public event EventHandler<(CCSPlayerPawn, int)> PlayerMoneyChangedEventHandler;
    public event EventHandler<CCSPlayerPawn> PlayerInventoryChangedEventHandler;
    public event EventHandler<(CCSPlayerPawn, CSTeamNumber, CSTeamNumber)> PlayerChangedTeamEventHandler;
    public event EventHandler<(CCSPlayerController, CCSPlayerController)> PlayerKillEventHandler; 

	
    /// <summary>
    /// Create callbacks for Events that might be interesting for us
    /// </summary>
	private void InitializeEntityEventHandlers()
	{
        
        Demo.Source1GameEvents.PlayerDeath += e =>
        {
            if (WannaSeek) return;
            if (e.AttackerPawn?.Controller != null && e.PlayerPawn?.Controller != null)
            {
                PlayerKillEventHandler?.Invoke(this, (e.AttackerPawn.Controller, e.PlayerPawn.Controller));
            }
            if (e.PlayerPawn?.Controller != null)
            {
                PlayerDeathEventHandler?.Invoke(this, e.PlayerPawn.Controller.EntityIndex.Value);
            }
            else
            {
                GD.PushWarning($"PlayerPawn null for DeathEvent {e}");
            }
        };

        Demo.Source1GameEvents.PlayerSpawn += e =>
        {
            if (WannaSeek) return;
            if (e.PlayerPawn != null)
            {
                PlayerRespawnEventHandler?.Invoke(this, (uint)e.PlayerPawn?.Controller?.EntityIndex.Value);
            }
            else
            {
                GD.PushWarning($"PlayerPawn null for RespawnEvent {e}");
            }
        };

        Demo.Source1GameEvents.WeaponFire += e =>
        {
            if (WannaSeek) return;
            PlayerShootEventHandler?.Invoke(this, (uint)e.PlayerPawn?.Controller?.EntityIndex.Value);
        };

        Demo.Source1GameEvents.PlayerShoot += e =>
        {
            if (WannaSeek) return;
            PlayerShootEventHandler?.Invoke(this, (uint)e.PlayerPawn?.Controller?.EntityIndex.Value);
        };

        Demo.Source1GameEvents.PlayerSpawned += e =>
        {
            if (WannaSeek) return;
            PlayerRespawnEventHandler?.Invoke(this, (uint)e.PlayerPawn?.Controller?.EntityIndex.Value);
        };

        Demo.EntityEvents.CCSPlayerPawn.AddChangeCallback(pawn => pawn.Health,
            (pawn, _, _) =>
            {
                if (WannaSeek) return;
                PlayerHealthChangedEventHandler?.Invoke(this, pawn);
            });

        Demo.EntityEvents.CCSPlayerPawn.AddChangeCallback(pawn => pawn.Weapons,
            (pawn, _, _) =>
            {
                if (WannaSeek) return;
                PlayerInventoryChangedEventHandler?.Invoke(this, pawn);
            }
        );

        Demo.EntityEvents.CCSPlayerController.AddChangeCallback(controller => controller.InGameMoneyServices?.Account,
            (controller, _, newMoney) =>
            {
                if (WannaSeek) return;
                if (newMoney == null) return;
                if (controller.PlayerPawn == null) return;
                PlayerMoneyChangedEventHandler?.Invoke(this, (controller.PlayerPawn, (int)newMoney));
            }
        );
        
        Demo.EntityEvents.CCSPlayerPawn.AddChangeCallback(pawn => pawn.CSTeamNum,
            (pawn, oldTeam, newTeam) =>
            {
                if (WannaSeek) return;
                PlayerChangedTeamEventHandler?.Invoke(this, (pawn, oldTeam, newTeam));
            }
        );


        
        


    }
}