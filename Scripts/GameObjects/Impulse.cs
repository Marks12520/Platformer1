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
	
	private void _on_body_entered(Node2D body)
	{
		if (body.Name == "Player")
		{
			CharacterBody2D character = (CharacterBody2D)body;
			Vector2 velocity = character.Velocity;
			if (direction == Direction.Left)
			{
				velocity.X = -amount;
			}
			else if (direction == Direction.Right)
			{
				velocity.X = amount;
			}
			character.Velocity = velocity;
		}
	}
}
