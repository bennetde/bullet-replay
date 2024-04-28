using Godot;
using System;

public partial class ShootSprite : Sprite2D
{
	private Timer _timer;

	public override void _Ready()
	{
		this.Visible = false;
		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += TimerEnded;
	}

	public void Shoot()
	{
		this.Visible = true;
		_timer.Start();
	}

	public void TimerEnded()
	{
		this.Visible = false;
	}
}
