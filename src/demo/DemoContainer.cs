using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DemoFile;
using DemoReplay.Demo.Stats;
using Godot;

namespace DemoReplay.Demo;

public partial class DemoContainer : Control
{
	public int CurrentTick { get; private set; }
	public int MaxTicks { get; private set; }
	public bool Paused { get; set; }
	public bool Ended { get; set; }
	public float PlaybackSpeed { get; set; }
    
	private DemoParser Demo { get; set; }
	
	private DemoStatistics Statistics { get; set; }
	
	// Stores the state of a search request to a specific tick
	private bool WannaSeek { get; set; }
	private int SeekedTick { get; set; }
	private int CurrentRoundIndex { get; set; }
	
	[Export]
	private HSlider DemoProgressBar { get; set; }
	
	[Export]
	private MapController MapController { get; set; }
	
	[Export]
	private PlayerEntitiesController PlayerController { get; set; }
	
	[Export]
	private GrenadeEntitiesController GrenadeController { get; set; }
	
	[Export]
	private MatchInfoPanel MatchInfoPanel { get; set; }
	
	private bool Ticked { get; set; }

	public DemoContainer()
	{
		Demo = new DemoParser();
		PlaybackSpeed = 1;
		// Wait for NetTick so we know when 
		Demo.PacketEvents.NetTick += _ =>
		{
			Ticked = true;
		};

		Demo.Source1GameEvents.RoundEnd += e =>
		{
			if (WannaSeek) return;
			CurrentRoundIndex += 1;
			SetRound(CurrentRoundIndex, false);
		};

		InitializeEntityEventHandlers();
		InitializeMatchEventHandlers();
		InitializePlayerEventHandlers();
		InitializeGrenadeEventHandlers();
	}

	/// <summary>
	/// Loads the demo file specified on `path`
	/// </summary>
	/// <param name="path">Path to the demo file</param>
	public async Task LoadDemo(string path)
	{
		GD.Print($"Loading Demo: {path}");
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		Statistics = await DemoStatistics.CreateDemoStatistics(path);
		stopwatch.Stop();
		OnStatsLoaded?.Invoke(this, Statistics);
		GD.Print($"Created statistics after {stopwatch.Elapsed}");
		await Demo.StartReadingAsync(File.OpenRead(path), default);
		await Read();
	}

    private async Task Read()
    {
        MapController.CallDeferred(MapController.MethodName.LoadMap, Demo.ServerInfo.MapName);
        MapLoadedEventHandler?.Invoke(this, Demo.ServerInfo.MapName);
        Name = Demo.ServerInfo.MapName;
        GD.Print("Reading");
        SetRound(0, false);
        while (!Ended)
        {
            // Seek to another tick requested
            if (WannaSeek && !Demo.IsReading)
            {
	            GrenadeController.Clear();
                await Demo.SeekToTickAsync(new DemoTick(SeekedTick), default);
                WannaSeek = false;
                SeekedTick = 0;
                Paused = false;
                Ended = false;
                OnSkip();
                continue;
            }
            
            // Wait for some time and check if still Paused
            if (Paused)
            {
                await Task.Delay(16);
                continue;
            }
            
            // HACK: Fixes playback speed actually going faster for 16x
            // It looks like Task.Delay is too slow?
            if (Math.Abs(PlaybackSpeed - 16.0f) < Mathf.Epsilon)
            {
                for (var i = 0; i < 16; i++)
                {
                    if (Ended) break;
                    Ended = !await Demo.MoveNextAsync(default);
                }

                await Task.Delay(1);

            }
            else
            {
                Ended = !await Demo.MoveNextAsync(default);
                if (Ticked)
                {
                    await Task.Delay((int)(16.0f / PlaybackSpeed));
                }
            }
			DemoProgressBar.SetDeferred("value", Demo.CurrentDemoTick.Value);
        }
        
        GD.Print("Finished reading Demo");
    }

    /// <summary>
    /// Called after the demo skipped to a tick.
    /// </summary>
    private void OnSkip()
    {
	    // GD.Print("Finished Skip. Resetting Player Entities.");
	    // GD.Print("Cleared");
    }
    
    public void PlaybackSpeedChanged(float newValue)
    {
	    PlaybackSpeed = newValue;
    }

    public void SetPaused(bool value)
    {
	    Paused = value;
    }

    public void Seek(int tick)
    {
	    SeekedTick = tick;
	    WannaSeek = true;
	    Paused = true;
    }

    public void SetRound(int roundIndex, bool seek = true)
    {
	    var round = Statistics.Rounds[roundIndex];
	    if (seek)
	    {
		    Seek(round.RoundStartTick);
	    }

	    CurrentRoundIndex = roundIndex;
	    DemoProgressBar.SetDeferred("min_value", round.RoundStartTick);
	    DemoProgressBar.SetDeferred("max_value", round.RoundEndTick);
	    DemoProgressBar.SetDeferred("value", round.RoundStartTick);
    }
}