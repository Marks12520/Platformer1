using Godot;
using System;
using System.Linq;

public partial class Collectible : Area2D
{
    private CpuParticles2D particles;
    private AnimatedSprite2D sprite;
    
    public override void _Ready()
    {
        particles = GetNode<CpuParticles2D>("CPUParticles2D");
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
        sprite.Animation = "Idle";
    }
    
    private void _on_body_entered(CharacterBody2D body)
    {
        if (sprite.Animation != "Collected")
        {
            particles.Emitting = true;
        }
        sprite.Animation = "Collected";
        SetDeferred("monitorable", false);

    }
}
