using Godot;
using System;

[Tool]
public partial class FallingSpike : AnimatableBody2D
{
	private Sprite2D sprite;
	private CollisionShape2D collision;
	
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		collision = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public override void _Process(double delta)
	{
		collision.Position = sprite.Position + new Vector2(0, 7.5f);
	}
}
