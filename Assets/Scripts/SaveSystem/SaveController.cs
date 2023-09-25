using Assets.Scripts.Models;
using Assets.Scripts.SaveSystem;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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
        // Init building research
        gameProgress.buildingResearch =
            gameProgress.buildingRegistry.Buildings
            .Select(b =>
                new ItemAvaliability
                {
                    guid = b.Guid,
                    isAvaliable = b.isAvaliableAtStart
                }
            ).ToList();

        var file = saveSystem.LoadLast<SaveFile>();

        if (file != null)
        {
            file.WriteToGameProgress(gameProgress);
        }
    }

    private void Save(SaveMode mode)
    {
        string dateStr = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
        string fileName = mode.HumanName() + "Save " + dateStr + ".json";
        var file = new SaveFile();

        file.characterName = "none";
        file.date = dateStr;
        file.type = mode.HumanName();
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
