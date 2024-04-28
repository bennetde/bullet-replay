using Godot;
using System;


/// <summary>
/// Represents a Source-Engine entity.
/// </summary>
public partial class Entity : Node2D
{
	public MapBoundaries Boundaries { get; set; }
	
	/// <summary>
	/// Sets the position for a entity.
	/// Input is a source engine coordinate that will be transformed according to MapBoundaries.
	/// </summary>
	/// <param name="pos">Position from the Source Engine</param>
	public void SetPosition(Vector2 pos)
	{
		pos.X -= Boundaries.PosX;
		pos.Y -= Boundaries.PosY;
		pos.X /= Boundaries.Scale;
		pos.Y /= -Boundaries.Scale;
		GlobalPosition = pos;
	}
	
	public void SetRotation(float newYaw)
	{
		GlobalRotationDegrees = newYaw + 90;
	}
}
