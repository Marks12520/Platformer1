using Godot;
using System;

public partial class AirstreamToggle : Area2D
{
	private AnimatedSprite2D as2d;

	public override void _Ready()
	{
		as2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	private void _on_body_entered(Node2D body)
	{
		if (body.Name == "Player")
		{
			UpwardsStream.Instance.AddToggle();
			as2d.Animation = "On";
			SetDeferred("monitoring", false);
			GD.Print(Name + " toggled");
		}
	}
}
