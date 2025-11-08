using Godot;
using System;

public partial class Collectible : Area2D
{
    private CpuParticles2D particles;
    private AnimatedSprite2D sprite;
    
    public override void _Ready()
    {
        particles = GetNode<CpuParticles2D>("CPUParticles2D");
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    
    private void _on_body_entered(CharacterBody2D body)
    {
        particles.Emitting = true;
        sprite.Hide();
    }
}
