using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
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
        public List<Resource> resources;


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

            resources = progress.resourceContainer.Resources;
        }

        public void WriteToGameProgress(GameProgress progress)
        {
            progress.fame = Fame;
            progress.buildings = buildings;
            places.ForEach(p => progress.AddBuilding(p.position, p.buildingGuid));

            progress.resourceContainer.Resources.Clear();
            progress.resourceContainer.AddResources(resources);
        }

        [Serializable]
        public class Place
        {
            public Vector3 position;
            public string buildingGuid;
        }
    }
}
