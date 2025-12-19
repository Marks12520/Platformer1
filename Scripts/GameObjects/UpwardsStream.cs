using Godot;
using System;

[Tool]
public partial class UpwardsStream : Area2D
{
	public static UpwardsStream Instance;
	
	public bool IsStreamActive = false;
	public bool IsPlayerInStream;
	private int togglesActive = 0;
	[Export] public int TogglesRequired = 3;
	
	private CollisionShape2D flow;
	private CpuParticles2D particles;
	[Export] private float flowHeight = -1.5f;

	public override void _Ready()
	{
		Instance = this;
		flow = GetNode<CollisionShape2D>("Flow");
		particles = GetNode<CpuParticles2D>("CPUParticles2D");
		
		Vector2 position = flow.Position;
		position.Y = flowHeight;
		flow.Position = position;
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			Vector2 position = flow.Position;
			position.Y = flowHeight;
			flow.Position = position;
		}
	}
	
	private void _on_body_entered(Node2D body)
	{
		if (body.Name == "Player")
		{
			IsPlayerInStream = true;
		}
	}

	private void _on_body_exited(Node2D body)
	{
		if (body.Name == "Player")
		{
			IsPlayerInStream = false;
		}
	}

	public void AddToggle()
	{
		togglesActive += 1;
		if (togglesActive >= TogglesRequired)
		{
			IsStreamActive = true;
			particles.Emitting = true;
		}
	}
}
