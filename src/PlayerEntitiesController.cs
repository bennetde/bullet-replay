using System;
using System.Collections.Generic;
using System.Linq;
using DemoFile.Sdk;
using DemoReplay.Demo;
using Godot;

namespace DemoReplay;

public partial class PlayerEntitiesController : Node
{
	[Export] private DemoContainer DemoContainer { get; set; }
	[Export] private MapController MapController { get; set; }
	[Export] private PackedScene EntityScene { get; set; }

	private Dictionary<uint, PlayerEntity> Entities { get; set; }

	public override void _Ready()
	{
		Entities = new Dictionary<uint, PlayerEntity>();
		DemoContainer.PlayerControllerCreatedEventHandler += OnEntityCreated;
		DemoContainer.PlayerControllerDeletedEventHandler += OnEntityDeleted;
		DemoContainer.PlayerControllerMovedEventHandler += OnEntityMoved;
		DemoContainer.PlayerDeathEventHandler += OnPlayerDeath;
		DemoContainer.PlayerRespawnEventHandler += OnPlayerRespawn;
		DemoContainer.PlayerControllerRotatedEventHandler += OnPlayerRotate;
		DemoContainer.PlayerShootEventHandler += OnPlayerShoot;
		DemoContainer.PlayerControllerHealthChangedEventHandler += OnPlayerHealthChange;
	}

	public void Clear()
	{
		foreach (var pair in Entities)
		{
			pair.Value.CallDeferred(Node.MethodName.QueueFree);
		}
		Entities.Clear();
	}

	public void SetPlayers(IEnumerable<CCSPlayerController> controllers)
	{
		foreach (var controller in controllers)
		{
			if (controller.PlayerPawn == null)
			{
				GD.Print($"{controller.PlayerName} has no Pawn");
				continue;
			}

			if (controller.PlayerName.Equals(""))
			{
				GD.PushWarning("Controller has no name");
				continue;
			}
			OnEntityCreated(null, controller);
		}
	}

	private void OnPlayerHealthChange(object sender, (CCSPlayerController, int) pair)
	{
		if (Entities.TryGetValue(pair.Item1.EntityIndex.Value, out var node2D))
		{
			node2D.CallDeferred(PlayerEntity.MethodName.SetHealth, pair.Item2);
		}
	}

	private void OnPlayerShoot(object sender, uint e)
	{
		if (Entities.TryGetValue(e, out var node2D))
		{
			node2D.CallDeferred(PlayerEntity.MethodName.Shoot);
		}
	}

	private void OnPlayerRotate(object sender, (CCSPlayerController, float) e)
	{
		if (Entities.TryGetValue(e.Item1.EntityIndex.Value, out var node2D))
		{
			node2D.CallDeferred(PlayerEntity.MethodName.OnRotate, -e.Item2);
		}
	}

	private void OnPlayerRespawn(object sender, uint e)
	{
		if (Entities.TryGetValue(e, out var node2D))
		{
			node2D.CallDeferred(PlayerEntity.MethodName.OnAlive);
		}
	}

	private void OnPlayerDeath(object sender, uint e)
	{
		if (Entities.TryGetValue(e, out var node2D))
		{
			node2D.CallDeferred(PlayerEntity.MethodName.OnDeath);
		}
	}

	private void CreateChild(uint entityIndex, string name, int teamIndex, Vector2 pos)
	{
		var newEntity = EntityScene.Instantiate() as PlayerEntity;
		AddChild(newEntity);
		Entities.Add(entityIndex, newEntity);
		var boundaries = MapController.GetMapBoundaries(MapController.CurrentMap);
		newEntity!.SetInitData(teamIndex, name, boundaries);
		newEntity!.SetPosition(pos);
	}

	private void OnEntityCreated(object sender, CCSPlayerController ctrl)
	{
		// Godot's Variant System can't convert Enums automatically so we do it here
		var teamIndex = ctrl.CSTeamNum switch
		{
			CSTeamNumber.Terrorist => 0,
			CSTeamNumber.CounterTerrorist => 1,
			_ => -1
		};
		// Ignore any spectator entities
		if (teamIndex == -1) return;

		if (ctrl.PlayerPawn == null)
		{
			GD.PushWarning($"{ctrl.PlayerPawn} has no origin");
			return;
		}
		var position = new Vector2(ctrl.PlayerPawn.Origin.X, ctrl.PlayerPawn.Origin.Y);
		CallDeferred(MethodName.CreateChild, ctrl.EntityIndex.Value, ctrl.PlayerName, teamIndex, position);
	}
	
	private void OnEntityDeleted(object sender, CCSPlayerController ctrl)
	{
		if (Entities.TryGetValue(ctrl.EntityIndex.Value, out var node2D))
		{
			node2D.CallDeferred(Node.MethodName.QueueFree);
			Entities.Remove(ctrl.EntityIndex.Value);
		}
	}
	
	private void OnEntityMoved(object sender, (CCSPlayerController, Vector2) eventArgs) {
		if (Entities.TryGetValue(eventArgs.Item1.EntityIndex.Value, out var node))
		{
			node.CallDeferred(PlayerEntity.MethodName.SetPosition, eventArgs.Item2);
		}
	}
}

// public class PlayerPawnMovedEventArgs
// {
// 	public CCSPlayerController pawn;
// 	public Vector2 newPos;
// }