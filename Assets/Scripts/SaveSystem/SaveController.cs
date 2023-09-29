using Assets.Scripts.Models;
using Assets.Scripts.SaveSystem;
using System;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Assets.Scripts.SaveSystem.SaveFile;

public class SaveController : MonoBehaviour
{
    [Header("Utilities")]
    public SaveSystem saveSystem;
    public HubController controller;

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
        gameProgress.placedBuildings.Clear();

        var file = saveSystem.LoadLast<SaveFile>();
        
        if (file != null)
        {
            controller.AddBuildings(file.places);
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
        file.places = controller.buildingPlaces.Select(p
            => new Place
            {
                position = p.transform.position,
                buildingGuid = p.building?.Guid
            }
        ).ToList();

        saveSystem.SaveData(file, fileName);
        saveSystem.DeleteOldFiles("AutoSave");
    }

    private enum SaveMode
    {
        Hand,
        Auto,
    }
}
