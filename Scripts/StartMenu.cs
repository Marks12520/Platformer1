using Godot;
using System;

public partial class StartMenu : Control
{
	private void _on_play_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/level1.tscn");
	}
}
