using Godot;
using System;
using Vector2 = Godot.Vector2;

public partial class Platform : AnimatableBody2D
{
	private bool isDescending;
	private Area2D playerArea;
	[Export] private Node2D target;
	[Export] private float speed = 4;

	public override void _Ready()
	{
		playerArea = GetNode<Area2D>("PlayerArea");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 targetPosition = target.GlobalPosition;
		
		if (isDescending)
		{
			if (GlobalPosition.DistanceTo(targetPosition) <= 2)
			{
				isDescending = false;
				playerArea.Monitoring = false;
				GlobalPosition = targetPosition;
				return;
			}
			Vector2 position = GlobalPosition;
			position += GlobalPosition.DirectionTo(targetPosition) * speed;
			GlobalPosition = position;
		}
	}

	private void _on_player_area_body_entered(Node2D body)
	{
		if (body.Name == "Player")
		{
			isDescending = true;
		}
	}

	private void _on_player_area_body_exited(Node2D body)
	{
		if (body.Name == "Player")
		{
			isDescending = false;
		}
	}
}
