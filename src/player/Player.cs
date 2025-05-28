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
	public Godot.Vector2 velocity;
	[Signal]
	public delegate void HitEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Gets screen size
		ScreenSize = GetViewportRect().Size;
		Hide();

		// Connect signal body_entered signal with method
		BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MovePlayer();

		AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		Position += velocity * (float)delta;
		Position = new Godot.Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		ChooseAnimation(animatedSprite2D);

	}

	public void MovePlayer()
	{
		velocity = Godot.Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}
	}

	public void ChooseAnimation(AnimatedSprite2D animatedSprite2D)
	{
		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipV = velocity.Y > 0;
		}
	}

	public void OnBodyEntered(Node2D body)
	{
		// Hides the player after it gets hit by enemy
		Hide();
		// Emits hit signal
		EmitSignal(SignalName.Hit);
		// Disabling the area's collision shape can cause an error if it happens in the middle of the engine's collision processing. Using set_deferred() tells Godot to wait to disable the shape until it's safe to do so.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Godot.Vector2 position)
	{
		Show();
		Position = position;
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}
