using Godot;
using System;
using DemoReplay.Demo;

/// <summary>
/// Represents a planted C4 Bomb Entity
/// </summary>
public partial class BombEntity : Entity
{
	[Export] private DemoContainer DemoContainer { get; set; }
	[Export] private Sprite2D Sprite { get; set; }
	
	

	public override void _Ready()
	{
		Sprite = GetNode<Sprite2D>("Sprite");
		DemoContainer.BombCreatedEventHandler += (_, e) =>
		{
			GD.Print("Loaded Bomb");
			Sprite.CallDeferred(CanvasItem.MethodName.SetVisible, true);
			CallDeferred(Entity.MethodName.SetPosition, new Vector2(e.Origin.X, e.Origin.Y));
		};

		DemoContainer.BombDeletedEventHandler += (_, e) =>
		{
			GD.Print("Deleted Bomb");
			Sprite.CallDeferred(CanvasItem.MethodName.SetVisible, false);
		};

		DemoContainer.BombMovedEventHandler += (_, e) =>
		{
			GD.Print("Move Bomb");
			CallDeferred(Entity.MethodName.SetPosition, new Vector2(e.Origin.X, e.Origin.Y));
		};

		DemoContainer.MapLoadedEventHandler += (_, e) =>
		{
			Boundaries = MapController.GetMapBoundaries(e);
		};
	}
	
	
}
