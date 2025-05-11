using Godot;
using System;
using System.Numerics;

public partial class Player : Area2D
{
	// Variables
	[Export]
	public int Speed { get; set; } = 200;
	public Godot.Vector2 Dir;
	public Godot.Vector2 ScreenSize;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Gets screen size
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Godot.Vector2 velocity = Godot.Vector2.Zero;

		Dir = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		velocity = Dir * Speed;

	}
}
