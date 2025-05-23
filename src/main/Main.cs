using Godot;
using System;

public partial class Main : Node2D
{
	// references mob scene
	[Export]
	public PackedScene MobScene { get; set; }
	// score of the game
	private int _score;

	public override void _Ready()
	{
		 //EmitSignal()
	}

	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
	}
}
