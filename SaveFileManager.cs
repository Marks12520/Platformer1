using Godot;
using System;

public partial class SaveFileManager : Node
{
    public static SaveFileManager Instance;
    private string savePath = "user://save.dat";

    public override void _Ready()
    {
        Instance = this;
    }
    
    public void SaveGame()
    {
        var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
        file.StoreVar(Global.Instance.UnlockedLevels.Duplicate());
        file.StoreVar(Global.Instance.CollectedFlowers.Duplicate());
        file.StoreVar(Global.Instance.Flowers);
        file.Close();
    }

    public void LoadGame()
    {
        if (FileAccess.FileExists(savePath))
        {
            var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
            var unlockedLevelsData = file.GetVar();
            var collectedFlowersData = file.GetVar();
            var flowersData = file.GetVar();
            file.Close();

            Global.Instance.UnlockedLevels = unlockedLevelsData.AsGodotDictionary<string, bool>();
            Global.Instance.CollectedFlowers = collectedFlowersData.AsGodotDictionary<string, bool>();
            Global.Instance.Flowers = flowersData.AsInt16();
        }
    }
}
