using Godot;
using System;

public record MapBoundaries(float PosX, float PosY, float Scale);

public partial class MapController : Node
{
	
	[Export]
	private Godot.Collections.Dictionary<string, PackedScene> MapsDict { get; set; }
	private Node LoadedMap { get; set; }
	public string CurrentMap { get; private set; }
    
	public void LoadMap(string map)
	{
		LoadedMap?.QueueFree();
		var mapToLoad = MapsDict[map];
		if (mapToLoad != null)
		{
			CurrentMap = map;
			var instantiatedMap = mapToLoad.Instantiate();
			AddChild(instantiatedMap);
			LoadedMap = instantiatedMap;
		}
		else
		{
			GD.PushWarning($"Couldn't load map {map.ToString()}");
		}
	}

	public static MapBoundaries GetMapBoundaries(string mapName)
	{
		return mapName switch
		{
			"de_ancient" => new MapBoundaries(-2953f, 2164f, 5f),
			"de_anubis" => new MapBoundaries(-2796f, 3328f, 5.22f),
			"de_inferno" => new MapBoundaries(-2087f, 3870f, 4.9f),
			"de_mirage" => new MapBoundaries(-3230f, 1713f, 5f),
			"de_nuke" => new MapBoundaries(-3453f, 2887f, 7f),
			"de_overpass" => new MapBoundaries(-4831f, 1781f, 5.2f),
			"de_vertigo" => new MapBoundaries(-3168f, 1762f, 4f),
			_ => new MapBoundaries(0, 0, 0)
		};
	}
}
