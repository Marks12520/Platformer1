using Godot;
using System;

public partial class GameMenu : Control
{
	private Label flowerLabel;
	
	public override void _Ready()
	{
		flowerLabel = GetNode<Label>("FlowerLabel");
	}
	
	public override void _Process(double delta)
	{
		flowerLabel.Text = "x " + Global.Instance.Flowers;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("mainmenu"))
		{
			GetTree().ChangeSceneToFile("res://Scenes/Menus/start_menu.tscn");
			SaveFileManager.Instance.SaveGame();
		}
	}
}
