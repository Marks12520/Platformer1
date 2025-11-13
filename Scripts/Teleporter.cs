using Godot;
using System;

public partial class Teleporter : Node2D
{
    private Area2D inputArea;
    private Area2D outputArea;
    private Timer cooldownTimer;

    [Export] public Vector2 exitPosition;

    private bool hasTeleported;
    
    public override void _Ready()
    {
        inputArea = GetNode<Area2D>("InputArea");
        outputArea = GetNode<Area2D>("OutputArea");
        cooldownTimer = GetNode<Timer>("CooldownTimer");
        
        outputArea.GlobalPosition = exitPosition;
    }
    
    private void _on_input_area_body_entered(Node2D body)
    {
        if (!hasTeleported)
        {
            body.Position = outputArea.GlobalPosition;
            cooldownTimer.Start();
            hasTeleported = true;
        }
    }
    
    private void _on_output_area_body_entered(Node2D body)
    {
        if (!hasTeleported)
        {
            body.Position = inputArea.GlobalPosition;
            cooldownTimer.Start();
            hasTeleported = true;
        }
    }

    private void _on_cooldown_timer_timeout()
    {
        hasTeleported = false;
    }
}
