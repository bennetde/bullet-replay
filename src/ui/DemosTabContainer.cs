using Godot;
using System;
using DemoReplay.Demo;

public partial class DemosTabContainer : TabContainer
{

	[Export] private PackedScene DemoContainer { get; set; }

	public override void _Ready()
	{
		var tabBar = GetTabBar();
		tabBar.TabCloseDisplayPolicy = TabBar.CloseButtonDisplayPolicy.ShowActiveOnly;
		tabBar.TabClosePressed += e => GetTabBar().RemoveTab((int)e);
		tabBar.DragToRearrangeEnabled = true;
	}
	
	public void OnFileSelected(string path)
	{
		var instantiatedDemoContainer = DemoContainer.Instantiate() as DemoContainer;
		if (instantiatedDemoContainer == null)
		{
			GD.PushError("Couldn't initialize Demo Container");
			return;
		}
		
		AddChild(instantiatedDemoContainer);
		instantiatedDemoContainer?.LoadDemo(path);

	}
}
