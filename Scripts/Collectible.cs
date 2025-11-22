using Godot;
using System;

public partial class Collectible : Area2D
{
    private CpuParticles2D particles;
    private AnimatedSprite2D sprite;
    private string name;
    
    public override void _Ready()
    {
        particles = GetNode<CpuParticles2D>("CPUParticles2D");
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        name = GetParent().GetParent().Name+"/"+Name;
        
        sprite.Animation = "Idle";

        if (Global.Instance.CollectedFlowers.ContainsKey(name) == false)
        {
            Global.Instance.CollectedFlowers.Add(name, false);
        }
        
        if (Global.Instance.CollectedFlowers[name])
        {
            sprite.Animation = "Collected";
            SetDeferred("monitorable", false);
        }
    }
    
    private void _on_body_entered(CharacterBody2D body)
    {
        if (sprite.Animation != "Collected")
        {
            particles.Emitting = true;
        }
        sprite.Animation = "Collected";
        SetDeferred("monitorable", false);
        
        Global.Instance.CollectedFlowers[name] = true;
    }
}
