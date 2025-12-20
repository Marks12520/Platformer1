using Godot;
using System;

public partial class Impulse : Area2D
{
	private enum Direction
	{
		Left,
		Right
	}
	
	[Export] private int amount;
	[Export] private Direction direction;
	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	private void _on_body_entered(Node2D body)
	{
		if (body.Name == "Player")
		{
			CharacterBody2D character = (CharacterBody2D)body;
			
			if (character.Velocity.X == 0)
			{
				animationPlayer.Play("Extract");
			}
			
			Vector2 velocity = character.Velocity;
			if (direction == Direction.Left)
			{
				velocity.X -= amount;
			}
			else if (direction == Direction.Right)
			{
				velocity.X += amount;
			}
			character.Velocity = velocity;

			
		}
	}
}
