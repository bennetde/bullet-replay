using Godot;
using System;
using DemoFile.Sdk;

public partial class PlayerEntity : Node2D
{
	[Export] private Color _terroristColor;
	[Export] private Color _counterTerroristColor;
	[Export] private Color _deathColor;

	private MapBoundaries _boundaries;
	
	[Export]
	private Sprite2D _sprite;
	[Export]
	private Sprite2D _deadSprite;
	private Label _label;
	
	[Export]
	private ShootSprite _shootSprite;

	private int _teamIndex;

	public void OnDeath()
	{
		_sprite.Visible = false;
		_deadSprite.Visible = true;
	}

	public void OnAlive()
	{
		_sprite.Visible = true;
		_deadSprite.Visible = false;
	}
	
	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
	}

	public void SetInitData(int teamIndex, String name, MapBoundaries boundaries)
	{
		_teamIndex = teamIndex;
		_sprite.Modulate = teamIndex == 0 ? _terroristColor : _counterTerroristColor;
		_deadSprite.Modulate = (teamIndex == 0 ? _terroristColor : _counterTerroristColor) * new Color(1,1,1, 0.5f);
		_label.Text = name;
		_boundaries = boundaries;
	}
	public void SetPosition(Vector2 pos)
	{
		pos.X -= _boundaries.PosX;
		pos.Y -= _boundaries.PosY;
		pos.X /= _boundaries.Scale;
		pos.Y /= -_boundaries.Scale;
		GlobalPosition = pos;
	}

	public void SetHealth(int newVal)
	{
		var healthPerc = newVal / 100.0f;
		(_sprite.Material as ShaderMaterial).SetShaderParameter("health_perc", healthPerc);
	}

	public void OnRotate(float newYaw)
	{
		_sprite.GlobalRotationDegrees = newYaw + 90;
	}

	public void Shoot()
	{
		_shootSprite.Shoot();
	}
}
