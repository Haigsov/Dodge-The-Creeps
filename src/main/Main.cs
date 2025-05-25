using Godot;
using System;

public partial class Main : Node2D
{
	// references mob scene
	[Export]
	public PackedScene MobScene { get; set; }
	public Player Player;
	// score of the game
	private int _score;

	public override void _Ready()
	{
		Player = GetNode<Player>("Player");
		Player.Hit += GameOverSignal;
	}

	public void GameOverSignal()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
	}
}
