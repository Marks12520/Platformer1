using Godot;
using System;
using System.ComponentModel;
using System.Diagnostics;

public partial class PlayerMovement : CharacterBody2D
{
	private float speed = 200.0f;
	private float jumpVelocity = -500.0f;
	private AnimatedSprite2D as2d;

	public override void _Ready()
	{
		as2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("Move_left", "Move_right", "Move_up", "Move_down");

		// Apply gravity
		if (!IsOnFloor())
		{
			velocity +=GetGravity() * (float)delta;
		}
		
		// Jump
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = jumpVelocity;
		}

		// Moving
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speed;
			FlipCharacter(direction);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
		}

		// Climbing
		//if ()
		
		Velocity = velocity;
		MoveAndSlide();
	}

	private void FlipCharacter(Vector2 direction)
	{
		if (MathF.Sign(direction.X) == 1)
		{
			as2d.FlipH = false;
		}
		else
		{
			as2d.FlipH = true;
		}
	}
}
