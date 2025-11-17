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
		coinLabel.Text = "Coins: " + Global.Instance.Coins;
	}
}
