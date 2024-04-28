using DemoReplay.Resources;
using Godot;
using Godot.Collections;

namespace DemoReplay.singletons;

/// <summary>
/// Godot autoloaded Class that associates weapon names with their respective UI sprites
/// </summary>
public partial class WeaponsDictionary : Resource
{
	[Export]
	public Dictionary<string, Weapon> Weapons { get; private set; }
}