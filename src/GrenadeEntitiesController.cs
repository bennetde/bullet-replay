using Godot;
using System;
using DemoReplay.Demo;
using DemoReplay.Entities;
using Godot.Collections;

namespace DemoReplay;

public partial class GrenadeEntitiesController : Node
{
	[Export] public DemoContainer DemoContainer { get; set; }
	[Export] private MapController MapController { get; set; }
	[Export] public PackedScene SmokeEntityScene { get; set; }
	[Export] public PackedScene HEEntityScene { get; set; }
	[Export] public PackedScene FlashEntityScene { get; set; }
	[Export] public PackedScene MolotovEntityScene { get; set; }

	private Dictionary<uint, SmokeGrenadeEntity> SmokeEntities { get; set; }
	private Dictionary<uint, HEGrenadeEntity> HEEntities { get; set; }
	private Dictionary<uint, FlashGrenadeEntity> FlashEntities { get; set; }
	private Dictionary<uint, MolotovGrenadeEntity> MolotovEntities { get; set; }

	public void Clear()
	{
		foreach (var pair in SmokeEntities)
		{
			pair.Value.CallDeferred(Node.MethodName.QueueFree);
		}
		foreach (var pair in HEEntities)
		{
			pair.Value.CallDeferred(Node.MethodName.QueueFree);
		}
		foreach (var pair in FlashEntities)
		{
			pair.Value.CallDeferred(Node.MethodName.QueueFree);
		}
		foreach (var pair in MolotovEntities)
		{
			pair.Value.CallDeferred(Node.MethodName.QueueFree);
		}
		SmokeEntities.Clear();
		HEEntities.Clear();
		FlashEntities.Clear();
		MolotovEntities.Clear();
	}

	public void CreateSmokeChild(uint id)
	{
		var entityIndex = id;
		var newEntity = SmokeEntityScene.Instantiate() as SmokeGrenadeEntity;
		AddChild(newEntity);
		SmokeEntities.Add(entityIndex, newEntity);
		var boundaries = MapController.GetMapBoundaries(MapController.CurrentMap);
		newEntity.MapBoundaries = boundaries;
	}
	
	public void CreateHEChild(uint id)
	{
		var entityIndex = id;
		var newEntity = HEEntityScene.Instantiate() as HEGrenadeEntity;
		AddChild(newEntity);
		HEEntities.Add(entityIndex, newEntity);
		var boundaries = MapController.GetMapBoundaries(MapController.CurrentMap);
		newEntity.Boundaries = boundaries;
	}
	
	public void CreateFlashChild(uint id)
	{
		var entityIndex = id;
		var newEntity = FlashEntityScene.Instantiate() as FlashGrenadeEntity;
		AddChild(newEntity);
		FlashEntities.Add(entityIndex, newEntity);
		var boundaries = MapController.GetMapBoundaries(MapController.CurrentMap);
		newEntity.Boundaries = boundaries;
	}
	
	public void CreateMolotovChild(uint id)
	{
		var entityIndex = id;
		var newEntity = MolotovEntityScene.Instantiate() as MolotovGrenadeEntity;
		AddChild(newEntity);
		MolotovEntities.Add(entityIndex, newEntity);
		var boundaries = MapController.GetMapBoundaries(MapController.CurrentMap);
		newEntity.Boundaries = boundaries;
	}
	
	public override void _Ready()
	{
		SmokeEntities = new();
		HEEntities = new();
		FlashEntities = new();
		MolotovEntities = new();
		DemoContainer.SmokeGrenadeCreatedEventHandler += (_, smoke) =>
		{
			CallDeferred(MethodName.CreateSmokeChild, smoke.EntityIndex.Value);
		};
		
		DemoContainer.HEGrenadeCreatedEventHandler += (_, nade) =>
		{
			CallDeferred(MethodName.CreateHEChild, nade.EntityIndex.Value);
		};
		
		DemoContainer.FlashGrenadeCreatedEventHandler += (_, nade) =>
		{
			CallDeferred(MethodName.CreateFlashChild, nade.EntityIndex.Value);
		};
		
		DemoContainer.MolotovGrenadeCreatedEventHandler += (_, nade) =>
		{
			CallDeferred(MethodName.CreateMolotovChild, nade.EntityIndex.Value);
		};

		DemoContainer.SmokeGrenadeMovedEventHandler += (_, smoke) =>
		{
			if (SmokeEntities.TryGetValue(smoke.EntityIndex.Value, out var smokeEntity))
			{
				var pos = new Vector2(smoke.Origin.X, smoke.Origin.Y);
				smokeEntity.CallDeferred(SmokeGrenadeEntity.MethodName.SetPosition, pos);
			}
		};
		
		DemoContainer.HEGrenadeMovedEventHandler += (_, nade) =>
		{
			if (HEEntities.TryGetValue(nade.EntityIndex.Value, out var nadeEntity))
			{
				var pos = new Vector2(nade.Origin.X, nade.Origin.Y);
				nadeEntity.CallDeferred(Entity.MethodName.SetPosition, pos);
			}
		};
		
		DemoContainer.FlashGrenadeMovedEventHandler += (_, nade) =>
		{
			if (FlashEntities.TryGetValue(nade.EntityIndex.Value, out var nadeEntity))
			{
				var pos = new Vector2(nade.Origin.X, nade.Origin.Y);
				nadeEntity.CallDeferred(Entity.MethodName.SetPosition, pos);
			}
		};
		
		DemoContainer.MolotovGrenadeMovedEventHandler += (_, nade) =>
		{
			if (MolotovEntities.TryGetValue(nade.EntityIndex.Value, out var nadeEntity))
			{
				var pos = new Vector2(nade.Origin.X, nade.Origin.Y);
				nadeEntity.CallDeferred(Entity.MethodName.SetPosition, pos);
			}
		};

		DemoContainer.SmokeGrenadeDeletedEventHandler += (_, smoke) =>
		{
			if (SmokeEntities.TryGetValue(smoke.EntityIndex.Value, out var node2D))
			{
				node2D.CallDeferred(Node.MethodName.QueueFree);
				SmokeEntities.Remove(smoke.EntityIndex.Value);
			}
		};
		
		DemoContainer.HEGrenadeDeletedEventHandler += (_, nade) =>
		{
			if (HEEntities.TryGetValue(nade.EntityIndex.Value, out var node2D))
			{
				node2D.CallDeferred(Node.MethodName.QueueFree);
				HEEntities.Remove(nade.EntityIndex.Value);
			}
		};
		
		DemoContainer.FlashGrenadeDeletedEventHandler += (_, nade) =>
		{
			if (FlashEntities.TryGetValue(nade.EntityIndex.Value, out var node2D))
			{
				node2D.CallDeferred(Node.MethodName.QueueFree);
				FlashEntities.Remove(nade.EntityIndex.Value);
			}
		};
		
		DemoContainer.MolotovGrenadeDeletedEventHandler += (_, nade) =>
		{
			if (MolotovEntities.TryGetValue(nade.EntityIndex.Value, out var node2D))
			{
				node2D.CallDeferred(Node.MethodName.QueueFree);
				MolotovEntities.Remove(nade.EntityIndex.Value);
			}
		};

		DemoContainer.SmokeGrenadePoppedEventHandler += (_, smoke) =>
		{
			if (SmokeEntities.TryGetValue(smoke.EntityIndex.Value, out var node2D))
			{
				node2D.CallDeferred(SmokeGrenadeEntity.MethodName.Pop);
			}
		};
	}

}
