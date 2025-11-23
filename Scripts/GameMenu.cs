using Godot;
using System;

public partial class GameMenu : Control
{
	private Label coinLabel;
	
	public override void _Ready()
	{
		coinLabel = GetNode<Label>("CoinLabel");
	}
	
	public override void _Process(double delta)
	{
		coinLabel.Text = "Flowers: " + Global.Instance.Flowers;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("mainmenu"))
		{
			GetTree().ChangeSceneToFile("res://Scenes/Menus/start_menu.tscn");
		}
	}
}
