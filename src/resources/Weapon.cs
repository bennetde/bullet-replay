using Godot;

namespace DemoReplay.Resources;


public enum GrenadeType
{
	None,
	HE,
	Flash,
	Smoke,
	Molotov,
	Decoy,
}

public partial class Weapon : Resource
{
	[Export]
	public string Label { get; set; }
	[Export]
	public CompressedTexture2D Texture { get; set; }
	[Export]
	public bool IsSecondary { get; set; }
	[Export]
	public GrenadeType GrenadeType { get; set; }
	[Export]
	public bool IsKnife { get; set; }
	
	[Export]
	public bool IsC4 { get; set; }
}