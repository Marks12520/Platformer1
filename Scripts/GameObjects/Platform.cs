using Godot;
using Vector2 = Godot.Vector2;

public partial class Platform : AnimatableBody2D
{
	[Export] private bool loop;
	
	private bool isDescending;
	private bool isAscending;
	private Area2D playerArea;
	
	[Export] private Node2D target;
	private Vector2 targetPosition;
	private Vector2 startPosition;
	private Timer ascendWait;
	private Timer descendWait;
	
	[Export] private float speed = 4;

	public override void _Ready()
	{
		playerArea = GetNode<Area2D>("PlayerArea");
		targetPosition = target.GlobalPosition;
		startPosition = GlobalPosition;
		ascendWait = GetNode<Timer>("AscendWait");
		descendWait = GetNode<Timer>("DescendWait");
		if (loop)
		{
			playerArea.Monitoring = false;
			isDescending = true;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 position = GlobalPosition;
		
		if (isDescending)
		{
			if (GlobalPosition.DistanceTo(targetPosition) <= 0)
			{
				isDescending = false;
				playerArea.Monitoring = false;
				if (loop)
				{
					ascendWait.Start();
				}
				return;
			}
			position += startPosition.DirectionTo(targetPosition) * speed;
		}

		if (isAscending)
		{
			if (GlobalPosition.DistanceTo(startPosition) <= 0)
			{
				isAscending = false;
				descendWait.Start();
				return;
			}
			position += targetPosition.DirectionTo(startPosition) * speed;
		}
		
		GlobalPosition = position;
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

	private void _on_ascend_wait_timeout()
	{
		isAscending = true;
	}

	private void _on_descend_wait_timeout()
	{
		isDescending = true;
	}
}
