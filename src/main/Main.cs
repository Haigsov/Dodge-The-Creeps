using Godot;
using System;

public partial class Main : Node2D
{
	// references mob scene
	[Export]
	public PackedScene MobScene { get; set; }
	// reference of player scene
	public Player Player;
	// score of the game
	private int _score;
	// timer variables
	private Timer StartTimer;
	private Timer MobTimer;
	private Timer ScoreTimer;

	public override void _Ready()
	{
		// get player node
		Player = GetNode<Player>("Player");
		StartTimer = GetNode<Timer>("StartTimer");
		MobTimer = GetNode<Timer>("MobTimer");
		ScoreTimer = GetNode<Timer>("ScoreTimer");
		// Connect hit signal with GameOverSignal method.
		Player.Hit += GameOverSignal;
		StartTimer.Timeout += OnStartTimerTimeoutSignal;
		ScoreTimer.Timeout += OnScoreTimerTimeoutSignal;
		MobTimer.Timeout += OnMobTimerTimeoutSignal;

		// Connect StartGame Signal to NewGame().
		GetNode<Hud>("HUD").StartGame += NewGame;
	}

	public void GameOverSignal()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();

		GetNode<Hud>("HUD").ShowGameOver();

		GetNode<AudioStreamPlayer2D>("Music").Stop();
        GetNode<AudioStreamPlayer2D>("DeathSound").Play();
	}

	public void NewGame()
	{
		_score = 0;

		Marker2D startPosition = GetNode<Marker2D>("StartPosition");
		Player.Start(startPosition.Position);

		StartTimer.Start();

		Hud hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		GetNode<AudioStreamPlayer2D>("Music").Play();

		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
	}

	private void OnStartTimerTimeoutSignal()
	{
		MobTimer.Start();
		ScoreTimer.Start();
	}

	private void OnScoreTimerTimeoutSignal()
	{
		_score++;
		GetNode<Hud>("HUD").UpdateScore(_score);
	}

	private void OnMobTimerTimeoutSignal()
	{
		// Create a new instance of the Mob scene.
		Mob mob = MobScene.Instantiate<Mob>();

		// Choose a random location on Path2D.
		PathFollow2D mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's direction perpendicular to the path direction.
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Set the mob's position to a random location.
		mob.Position = mobSpawnLocation.Position;

		// Add some randomness to the direction.
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		// Chose the velocity.
		Vector2 velocity = new Vector2((float)GD.RandRange(150, 250), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
	}
}
