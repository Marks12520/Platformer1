using Godot;
using System;
using System.Text.RegularExpressions;
using PhantomCamera;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void StartSnowEventHandler();
	
	private float speed = 200.0f;
	private float jumpVelocity = -500.0f;
	private float bounceAmount = -900.0f;
	
	private AnimatedSprite2D as2d;
	private CpuParticles2D deathParticles;
	private Timer deathTimer;
	private Timer idleTimer;
	private Timer fadeInTimer;
	
	[Export] private Node2D nextSpawn;
	[Export] private Node2D previousSpawn;
	[Export] private Node2D deathSpawn;
	[Export] private FadeAnimation fadeAnimation;
	[Export] private Cameras cameras;

	private string currentScene;
	private int currentSceneNum;
	private string nextScenePath;
	private int lastScene;

	private bool allowClimb;
	private bool allowMovement;
	private bool isDead;

	private Node healthComponent;

	public override void _Ready()
	{
		as2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		deathParticles = GetNode<CpuParticles2D>("DeathParticles");
		deathTimer = GetNode<Timer>("DeathTimer");
		idleTimer = GetNode<Timer>("IdleTimer");
		currentScene = GetTree().CurrentScene.Name;
		currentSceneNum = int.Parse(Regex.Match(currentScene, @"\d+").Value);
		fadeInTimer = GetNode<Timer>("FadeInTimer");
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
		
		if (Global.Instance.UnlockedLevels.ContainsKey(currentScene) == false)
		{
			Global.Instance.UnlockedLevels.Add(currentScene, true);
		}
		
		Global.Instance.JustDied = false;
		allowMovement = false;
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
		if (Input.IsActionJustPressed("Jump") && IsOnFloor() && !isDead && allowMovement)
		{
			velocity.Y = jumpVelocity;
		}
		
		// Moving
		if (direction != Vector2.Zero && !isDead && allowMovement)
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
		if (allowClimb && direction != Vector2.Zero && allowMovement)
		{
			velocity.Y = direction.Y * speed;
		}
		else if (allowClimb)
		{
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
		}
		
		// Upwards (use getaprent.hasnode to check if the stream exists
		if (UpwardsStream.Instance.isPlayerInStream && UpwardsStream.Instance.isStreamActive)
		{
			GD.Print("Player is in stream");
			velocity.Y -= 50;
		}
		
		HandleAnimations(direction);
		
		if (Input.IsAnythingPressed())
		{
			cameras.SetIdleCameraPriority(0);
		}
		
		Velocity = velocity;
		MoveAndSlide();
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
			fadeInTimer.Start();
			Global.Instance.LastScene = currentSceneNum;
			nextScenePath = "res://Scenes/Levels/level" + (currentSceneNum + 1) + ".tscn";
			fadeAnimation.PlayFadeInAnimation();
		}
		else if (area.Name == "PreviousLevelTransition" && currentSceneNum != 1)
		{
			fadeInTimer.Start();
			Global.Instance.LastScene = currentSceneNum;
			nextScenePath = "res://Scenes/Levels/level" + (currentSceneNum - 1) + ".tscn";
			fadeAnimation.PlayFadeInAnimation();
		}
		
		if (area.Name == "SnowStart"){EmitSignalStartSnow();}
	}

	private void _on_death_timer_timeout()
	{
		GD.Print("Reloading scene...");
		GetTree().ReloadCurrentScene();
	}

	private void _on_idle_timer_timeout()
	{
		GD.Print("Player is idle");
		cameras.SetIdleCameraPriority(2);
	}

	private void _on_fade_in_timer_timeout()
	{
		GetTree().ChangeSceneToFile(nextScenePath);
	}

	private void _on_fade_out_timer_timeout()
	{
		allowMovement = true;
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
			if (allowMovement)
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
					//as2d.Animation = "Jump";
				}
				else
				{
					as2d.Animation = "Idle";
				}
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
		fadeAnimation.PlayFadeDeathAnimation();
	}
}