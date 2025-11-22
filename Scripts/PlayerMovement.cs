using Godot;
using System;
using System.Text.RegularExpressions;
using PhantomCamera;

public partial class PlayerMovement : CharacterBody2D
{
	private float speed = 200.0f;
	private float jumpVelocity = -500.0f;
	private float bounceAmount = -900.0f;
	
	private AnimatedSprite2D as2d;
	private AudioStreamPlayer2D jumpAudioPlayer;
	private CpuParticles2D deathParticles;
	private Timer deathTimer;
	private Timer idleTimer;
	private Timer levelTransitionTimer;
	
	[Export] private Node2D nextSpawn;
	[Export] private Node2D previousSpawn;
	[Export] private Node2D deathSpawn;
	[Export] private Node2D idleCamera;
	[Export] private Node2D zoomCamera;

	private string currentScene;
	private int currentSceneNum;
	private string nextScenePath;
	private int lastScene;

	private bool allowClimb;
	private bool isDead;

	private Node healthComponent;

	public override void _Ready()
	{
		as2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		jumpAudioPlayer = GetNode<AudioStreamPlayer2D>("JumpAudioPlayer");
		deathParticles = GetNode<CpuParticles2D>("DeathParticles");
		deathTimer = GetNode<Timer>("DeathTimer");
		idleTimer = GetNode<Timer>("IdleTimer");
		currentScene = GetTree().CurrentScene.Name;
		currentSceneNum = int.Parse(Regex.Match(currentScene, @"\d+").Value);
		levelTransitionTimer = GetNode<Timer>("LevelTransitionTimer");
		lastScene = Global.Instance.LastScene;
		healthComponent = GetNode("HealthComponent");

		if (Global.Instance.JustDied)
		{
			Position = deathSpawn.Position;
		}
		else if (Global.Instance.LastScene > currentSceneNum)
		{
			Position = previousSpawn.Position;
		}
		else if (Global.Instance.LastScene < currentSceneNum)
		{
			Position = nextSpawn.Position;
		}

		Global.Instance.JustDied = false;
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
			idleTimer.Start();
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
		
		if (Input.IsAnythingPressed())
		{
			idleCamera.AsPhantomCamera2D().Priority = 0;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("test"))
		{
			GD.Print(Global.Instance.CollectedFlowers);
		}
	}
	
	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body.Name == "LadderLayer") {allowClimb = true;}
		
		if (body.Name == "BounceLayer")
		{
			Vector2 velocity = Velocity;
			velocity.Y = bounceAmount;
			Velocity = velocity;
		}

		if (body.Name == "SpikesLayer") { healthComponent.Call("Damage", 100); }
		if (Global.Instance.RemoveNumbers(body.Name) == "FallingSpike") { healthComponent.Call("Damage", 100);}
	}
	
	private void _on_area_2d_body_exited(Node2D body)
	{
		if (body.Name == "LadderLayer") {allowClimb = false;}
	}

	private void _on_area_2d_area_entered(Area2D area)
	{
		if (Global.Instance.RemoveNumbers(area.Name) == "Flower")
		{
			Global.Instance.Flowers += 1;
		}

		if (area.Name == "NextLevelTransition")
		{
			levelTransitionTimer.Start();
			Global.Instance.LastScene = currentSceneNum;
			nextScenePath = "res://Scenes/Levels/level" + (currentSceneNum + 1) + ".tscn";
			//TODO fade animation
		}
		else if (area.Name == "PreviousLevelTransition" && currentSceneNum != 1)
		{
			levelTransitionTimer.Start();
			Global.Instance.LastScene = currentSceneNum;
			nextScenePath = "res://Scenes/Levels/level" + (currentSceneNum - 1) + ".tscn";
			//TODO fade animation
		}
	}

	private void _on_death_timer_timeout()
	{
		GD.Print("Reloading scene...");
		GetTree().ReloadCurrentScene();
	}

	private void _on_idle_timer_timeout()
	{
		GD.Print("Player is idle");
		idleCamera.AsPhantomCamera2D().Priority = 2;
	}

	private void _on_level_transition_timer_timeout()
	{
		GetTree().ChangeSceneToFile(nextScenePath);
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

	private void HandleDeath()
	{
		deathParticles.Emitting = true;
		if (isDead == false)
		{
			deathTimer.Start();
		}
		isDead = true;
		Global.Instance.JustDied = true;
	}
}