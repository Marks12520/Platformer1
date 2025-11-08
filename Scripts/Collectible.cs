using Godot;
using System;

public partial class Collectible : Area2D
{
    private void _on_body_entered(CharacterBody2D body)
    {
        GD.Print("+1 coin");
    }
}
