using Godot;
using System;

public partial class StartMenu : Control
{
	private void _on_play_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Levels/level1.tscn");
	}

	private void _on_settings_pressed()
	{
		//TODO settings page
	}
	
	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}
