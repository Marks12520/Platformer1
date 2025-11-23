using Godot;
using System;
using System.Text.RegularExpressions;
using Godot.Collections;

public partial class Global : Node
{
    public static Global Instance;
    
    public int Health;
    public int LastScene;
    public int Flowers;
    public bool JustDied;
    public Dictionary<string, bool> CollectedFlowers;
    public Dictionary<string, bool> UnlockedLevels;

    public override void _Ready()
    {
        Instance = this;
        Health = 100;
        Flowers = 0; 
        JustDied = false;

        CollectedFlowers = new(){};
        UnlockedLevels = new(){};
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("dictionary"))
        {
            GD.Print(Global.Instance.CollectedFlowers);
            GD.Print(Global.Instance.UnlockedLevels);
        }
    }
    
    public string RemoveNumbers(string text) => Regex.Replace(text, @"\d+", "");
}
