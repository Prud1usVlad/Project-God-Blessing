﻿using Assets.Scripts.Models;
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
        public string characterName;
        public string type;
        public int fame;
        public int day;
        public string date;

        // Progress
        public List<ItemAvaliability> reserchedBuildings;

        // Hub config
        public List<Place> places;
        public List<Resource> resources;


        public void ReadFromGameProgress(GameProgress progress)
        {
            day = progress.day;
            fame = progress.fame;
            reserchedBuildings = progress.buildingResearch;
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
            progress.fame = fame;

            foreach (var b in reserchedBuildings)
            {
                var item = progress.buildingResearch
                    .Find(o => b.guid == o.guid);

                item.isAvaliable = b.isAvaliable;
            }

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
