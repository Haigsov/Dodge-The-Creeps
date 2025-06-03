using Godot;
using System;

public partial class Hud : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    public override void _Ready()
    {
        GetNode<Button>("StartButton").Pressed += OnStartButtonPressedSignal;
        GetNode<Timer>("MessageTimer").Timeout += OnMessageTimerTimeoutSignal;
    }

    // Function that allows you to change the text in Message label
    public void ShowMessage(string text)
    {
        Label message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();

        GetNode<Timer>("MessageTimer").Start();
    }

    // Starts Game Over! sequence and then lets you replay the game again.
    async public void ShowGameOver()
    {
        ShowMessage("Game Over!");

        Timer messageTimer = GetNode<Timer>("MessageTimer");
        await ToSignal(messageTimer, Timer.SignalName.Timeout);

        ShowMessage("Dodge the Creeps!");

        // Created a timer within code instead of creating a new timer as a node.
        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        GetNode<Button>("StartButton").Show();
    }

    // Takes ScoreLabel and updates it.
    public void UpdateScore(int score)
    {
        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    // Hides StartButton and starts game when StartButton is pressed.
    private void OnStartButtonPressedSignal()
    {
        GetNode<Button>("StartButton").Hide();
        EmitSignal(SignalName.StartGame);
    }

    // Hides the Message when timer finishes countdown through timeout signal.
    private void OnMessageTimerTimeoutSignal()
    {
        GetNode<Label>("Message").Hide();
    }
}
