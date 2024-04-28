using Godot;
using System;
using DemoReplay.Demo;

public partial class ProgressBar : HSlider
{
	[Export] private DemoContainer _controller;
	// [Export] private Label label;
	public override void _Ready()
	{
		ValueChanged += OnValueChanged;
	}

	private void OnValueChanged(double value)
	{
		// label.Text = $"{value}/{MaxValue}";
	}

	public void OnDragStarted()
	{
		_controller.SetPaused(true);
	}

	public void OnDragEnded(bool valueChanged)
	{
		_controller.Seek((int)Value);
	}
}
