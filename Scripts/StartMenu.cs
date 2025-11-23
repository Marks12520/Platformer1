using Godot;
using System;

public partial class StartMenu : Control
{
	private void _on_play_pressed()
	{
		//TODO open level select page
	}

	private void _on_settings_pressed()
	{
		//TODO settings page
	}
	
	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	
	//! Levels
	private void _on_level_1_pressed()
	{
		string level = "level1";
		if (Global.Instance.UnlockedLevels.ContainsKey(level) && Global.Instance.UnlockedLevels[level])
		{
			GetTree().ChangeSceneToFile("res://Scenes/Levels/" + level + ".tscn");
		}
	}
	
	private void _on_level_2_pressed()
	{
		string level = "level2";
		if (Global.Instance.UnlockedLevels.ContainsKey(level) && Global.Instance.UnlockedLevels[level])
		{
			GetTree().ChangeSceneToFile("res://Scenes/Levels/" + level + ".tscn");
		}
	}
}
