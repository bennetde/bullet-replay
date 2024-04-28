using System;
using DemoReplay.Demo.Stats;

namespace DemoReplay.Demo;

public partial class DemoContainer
{
	public EventHandler<DemoStatistics> OnStatsLoaded { get; set; }
}