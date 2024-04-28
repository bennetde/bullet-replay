using Godot;

namespace DemoReplay.Entities;

/**
 * Represents a Smoke Grenade Entity
 */
public partial class SmokeGrenadeEntity : Node2D
{
	public MapBoundaries MapBoundaries { get; set; }
	
	private Sprite2D IconSprite { get; set; }
	private Sprite2D SmokeSprite { get; set; }
	private Timer PoppedTimer { get; set; }
	
	private Label Label { get; set; }

	public override void _Ready()
	{
		IconSprite = GetNode<Sprite2D>("SmokeIcon");
		SmokeSprite = GetNode<Sprite2D>("PoppedSmoke");
		PoppedTimer = GetNode<Timer>("PoppedTimer");
		Label = GetNode<Label>("Label");
	}

	public void SetPosition(Vector2 pos)
	{
		pos.X -= MapBoundaries.PosX;
		pos.Y -= MapBoundaries.PosY;
		pos.X /= MapBoundaries.Scale;
		pos.Y /= -MapBoundaries.Scale;
		GlobalPosition = pos;
	}

	public void Pop()
	{
		IconSprite.Visible = false;
		SmokeSprite.Visible = true;
		// Label.Visible = true;
		PoppedTimer.Start();
	}

	public void Timeout()
	{
		SmokeSprite.Visible = false;
		// Label.Visible = false;
	}

	public override void _Process(double delta)
	{
		// Label.Text = PoppedTimer.TimeLeft.ToString();
	}
}