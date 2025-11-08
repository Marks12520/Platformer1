using Godot;
using System;
using System.ComponentModel;
using System.Diagnostics;

public partial class PlayerMovement : CharacterBody2D
{
	private float speed = 200.0f;
	private float jumpVelocity = -500.0f;
	
	private AnimatedSprite2D as2d;
	
	private AudioStreamPlayer2D jumpAudioPlayer;
	private CpuParticles2D deathParticles;
	private Timer deathTimer;

	private bool allowClimb;
	private bool isDead;

	public override void _Ready()
	{
		as2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		jumpAudioPlayer = GetNode<AudioStreamPlayer2D>("JumpAudioPlayer");
		deathParticles = GetNode<CpuParticles2D>("DeathParticles");
		deathTimer = GetNode<Timer>("DeathTimer");
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
		if (Input.IsActionJustPressed("Jump") && IsOnFloor() && !isDead)
		{
			velocity.Y = jumpVelocity;
			if (allowClimb == false)
			{
				jumpAudioPlayer.Play();
			}
		}

		// Moving
		if (direction != Vector2.Zero && !isDead)
		{
			velocity.X = direction.X * speed;
			FlipCharacter(direction);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
		}
		
		// Climbing
		if (allowClimb && direction != Vector2.Zero)
		{
			velocity.Y = direction.Y * speed;
		}
		else if (allowClimb)
		{
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
		}
		
		HandleAnimations(direction);
		
		Velocity = velocity;
		MoveAndSlide();
	}
	
	private void _on_area_2d_body_entered(TileMapLayer TML)
	{
		if (TML.Name == "LadderLayer") {allowClimb = true;}
		
		if (TML.Name == "EffectorsLayer")
		{
			Vector2 velocity = Velocity;
			velocity.Y -= 850;
			Velocity = velocity;
		}

		if (TML.Name == "DangersLayer")
		{
			isDead = true;
			deathParticles.Emitting = true;
			deathTimer.Start();
		}
	}
	
	private void _on_area_2d_body_exited(TileMapLayer TML)
	{
		if (TML.Name == "LadderLayer") {allowClimb = false;}
	}

	private void _on_area_2d_area_entered(Area2D area)
	{
		if (area.Name == "Collectible")
		{
			GD.Print("Collected coin!");
		}
	}

	private void _on_death_timer_timeout()
	{
		GD.Print("Restarting level...");
		GetTree().ReloadCurrentScene();
	}
	
	private void FlipCharacter(Vector2 direction)
	{
		if (!allowClimb)
		{
			if (MathF.Sign(direction.X) > 0)
			{
				as2d.FlipH = false;
			}
			else if (MathF.Sign(direction.X) < 0)
			{
				as2d.FlipH = true;
			}
		}
	}

	private void HandleAnimations(Vector2 direction)
	{
		if (!isDead)
		{
			if (allowClimb && direction.Y != 0)
			{
				as2d.Animation = "Climb";
			}
			else if (allowClimb && direction.Y == 0)
			{
				as2d.Animation = "IdleClimb";
			}
			else if (!allowClimb && direction.X != 0)
			{
				as2d.Animation = "Run";
			}
			else if (!IsOnFloor())
			{
				as2d.Animation = "Jump";
			}
			else
			{
				as2d.Animation = "Idle";
			}
		}
		else
		{
			as2d.Animation = "Death";
		}
	}
	
	//I don't give a fuck about this branch
}
