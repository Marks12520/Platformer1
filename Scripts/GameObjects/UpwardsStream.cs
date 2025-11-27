using Godot;
using System;

public partial class UpwardsStream : Area2D
{
	public static UpwardsStream Instance;
	
	public bool IsStreamActive = false;
	public bool IsPlayerInStream;
	public int TogglesActive = 0;
	[Export] public int TogglesRequired = 3;

	public override void _Ready()
	{
		Instance = this;
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
		TogglesActive += 1;
		if (TogglesActive == TogglesRequired)
		{
			IsStreamActive = true;
			GD.Print("Stream is active");
		}
	}
}
