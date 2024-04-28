using Godot;
using Godot.Collections;

namespace DemoReplay.singletons;

/// <summary>
/// Godot autoloaded Class that associates weapon names with their respective UI sprites
/// </summary>
public partial class WeaponSpriteDictionary : Resource
{
	[Export]
	public Dictionary<string, CompressedTexture2D> Textures { get; private set; }
}