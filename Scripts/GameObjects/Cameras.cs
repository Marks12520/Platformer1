using Godot;
using System;
using PhantomCamera;

//[Tool]
public partial class Cameras : Node
{
	private Node2D followCamera;
	private Node2D idleCamera;
	[Export] private Node2D followTarget;
	[Export] private TileMapLayer limitTilemap;
	[Export] private Vector4I limitOffset;
	
	public override void _Ready()
	{
		followCamera = GetNode<Node2D>("FollowCamera");
		idleCamera = GetNode<Node2D>("IdleCamera");

		SetStuff();
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			SetStuff();
		}
	}
	
	public void SetIdleCameraPriority(int priority)
	{
		idleCamera.AsPhantomCamera2D().Priority = priority;
	}

	private void SetStuff()
	{
		if (followTarget == null || limitTilemap == null || followCamera == null || idleCamera == null) {return;}
		followCamera.AsPhantomCamera2D().FollowTarget = followTarget;
		idleCamera.AsPhantomCamera2D().FollowTarget = followTarget;

		followCamera.AsPhantomCamera2D().LimitTarget = limitTilemap.GetPath();
		idleCamera.AsPhantomCamera2D().LimitTarget = limitTilemap.GetPath();

		followCamera.AsPhantomCamera2D().LimitMargin = limitOffset;
		idleCamera.AsPhantomCamera2D().LimitMargin = limitOffset;
	}
}
