using Godot;

[Tool]
public partial class Teleporter : Node2D
{
    private Area2D inputArea;
    private Area2D outputArea;
    private Timer cooldownTimer;
    private AnimatedSprite2D inputSprite;
    private AnimatedSprite2D outputSprite;

    [Export] public Node2D ExitPosition;

    private bool hasTeleported;
    
    public override void _Ready()
    { 
        
        inputArea = GetNode<Area2D>("InputArea");
        outputArea = GetNode<Area2D>("OutputArea");
        cooldownTimer = GetNode<Timer>("CooldownTimer");
        inputSprite = GetNode<AnimatedSprite2D>("InputArea/InputAnimatedSprite2D");
        outputSprite = GetNode<AnimatedSprite2D>("OutputArea/OutputAnimatedSprite2D");
    }

    public override void _Process(double delta)
    { 
        outputArea.GlobalPosition = ExitPosition.GlobalPosition;
    }
    
    private void _on_input_area_body_entered(Node2D body)
    {
        if (!hasTeleported)
        {
            Teleport(outputArea.GlobalPosition, body);
        }
    }
    
    private void _on_output_area_body_entered(Node2D body)
    {
        if (!hasTeleported)
        {
            Teleport(inputArea.GlobalPosition, body);
        }
    }

    private void _on_cooldown_timer_timeout()
    {
        hasTeleported = false;
        inputSprite.Animation = "Standby";
        outputSprite.Animation = "Standby";
    }

    private void Teleport(Vector2 position, Node2D body)
    {
        body.Position = position;
        cooldownTimer.Start();
        hasTeleported = true;
        inputSprite.Animation = "Recharging";
        outputSprite.Animation = "Recharging";
    }
}
