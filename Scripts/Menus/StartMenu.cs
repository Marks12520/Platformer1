using Godot;
using System;

public partial class StartMenu : Control
{
	public override void _Ready()
	{
		SaveFileManager.Instance.LoadGame();
	}

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

	private void _on_delete_pressed()
	{
		SaveFileManager.Instance.DeleteSave();
	}

	private void _on_fullscreen_pressed()
	{
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
	}
	
	//! Levels
	private void _on_level_1_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Levels/level1.tscn");
	}
	
	private void _on_level_2_pressed()
	{
		string level = "level2";
		if (Global.Instance.UnlockedLevels.ContainsKey(level) && Global.Instance.UnlockedLevels[level])
		{
			GetTree().ChangeSceneToFile("res://Scenes/Levels/" + level + ".tscn");
		}
	}

	private void _on_level_3_pressed()
	{
		string level = "level3";
		if (Global.Instance.UnlockedLevels.ContainsKey(level) && Global.Instance.UnlockedLevels[level])
		{
			GetTree().ChangeSceneToFile("res://Scenes/Levels/" + level + ".tscn");
		}
	}
}
