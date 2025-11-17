using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Global : Node
{
    public static Global Instance { get; private set; }
    
    public int Health { get; set; }
    public int LastScene { get; set; }
    public int Coins { get; set; }
    public int CoinsBeforeChangingLevel { get; set; }

    public override void _Ready()
    {
        Instance = this;
        Health = 100;
        Coins = 0;
        CoinsBeforeChangingLevel = 0;
    }

    public string RemoveNumbers(string text) => Regex.Replace(text, @"\d+", "");
}
