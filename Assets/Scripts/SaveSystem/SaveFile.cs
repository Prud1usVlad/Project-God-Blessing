using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.SaveSystem
{
    [Serializable]
    public class SaveFile
    {
        // Header
        public string CharacterName { get; set; }
        public string Type { get; set; }
        public int Fame { get; set; }
        public string Date { get; set; }

        // Progress
        public List<ItemAvaliability> buildings { get; set; }

        // Hub config
        public List<Place> places;


        public void ReadFromGameProgress(GameProgress progress)
        {
            Fame = progress.fame;
            buildings = progress.buildings;
            places = progress.buildingsPlaces.Select(p 
                => new Place 
                    { 
                        position = p.transform.position, 
                        buildingGuid = p.building?.Guid 
                    }
                ).ToList();
        }

        public void WriteToGameProgress(GameProgress progress)
        {
            progress.fame = Fame;
            progress.buildings = buildings;
            places.ForEach(p => progress.AddBuilding(p.position, p.buildingGuid));
        }

        [Serializable]
        public class Place
        {
            public Vector3 position;
            public string buildingGuid;
        }
    }
}
