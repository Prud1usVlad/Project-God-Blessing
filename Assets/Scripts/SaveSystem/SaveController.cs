using Assets.Scripts.SaveSystem;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    [Header("Utilities")]
    public SaveSystem saveSystem;

    [Header("Data to save and load")]
    public GameProgress gameProgress;

    public void AutoSave() { Save(SaveMode.Auto); }
    public void HandSave() { Save(SaveMode.Hand); }

    public void Load()
    {
        var file = saveSystem.LoadLast<SaveFile>();

        if (file != null )
        {
            file.WriteToGameProgress(gameProgress);
        }
    }

    private void Save(SaveMode mode)
    {
        string dateStr = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
        string fileName = mode.HumanName() + "Save " + dateStr + ".json";
        var file = new SaveFile();

        file.CharacterName = "none";
        file.Date = dateStr;
        file.Type = mode.HumanName();
        file.ReadFromGameProgress(gameProgress);
        
        saveSystem.SaveData(file, fileName);
        saveSystem.DeleteOldFiles("AutoSave");
    }

    private enum SaveMode
    {
        Hand,
        Auto,
    }
}
