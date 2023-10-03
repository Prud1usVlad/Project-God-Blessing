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
        public string characterName;
        public string type;
        public int fame;
        public int lies;
        public int day;
        public string date;

        // Progress
        public List<ItemAvaliability> reserchedBuildings;

        // Hub config
        public List<Place> places;
        public List<Resource> resources;
        public List<Curse> curses;


        public void ReadFromGameProgress(GameProgress progress)
        {
            day = progress.day;
            fame = progress.fameTranslation.currentPoints;
            lies = progress.liesTranslation.currentPoints;
            reserchedBuildings = progress.buildingResearch;

            resources = progress.resourceContainer.Resources;
            curses = progress.curses.Select(c => 
                new Curse 
                { 
                    guid = c.Guid, 
                    imageIdx = c.imageIdx, 
                    prophesyIdx = c.prophesyIdx 
                }
            ).ToList();
        }

        public void WriteToGameProgress(GameProgress progress)
        {
            // set basic values
            progress.fameTranslation.SetPoints(fame);
            progress.liesTranslation.SetPoints(lies);

            // init all reserched buildings
            foreach (var b in reserchedBuildings)
            {
                var item = progress.buildingResearch
                    .Find(o => b.guid == o.guid);

                item.isAvaliable = b.isAvaliable;
            }

            // set resources
            progress.resourceContainer.Resources.Clear();
            progress.resourceContainer.AddResources(resources);

            // set curses
            curses.ForEach(c => progress.curses.Add(progress.curseRegistry
                .InitByGuid(c.guid, c.prophesyIdx, c.imageIdx)));

            // set modifiers
            foreach (var c in progress.curses)
                progress.globalModifiers.AddModifiers(c);

            foreach (var b in progress.placedBuildings)
                if (b is BonusBuilding)
                    progress.globalModifiers.AddModifiers((BonusBuilding)b);

            // TODO: in editor set up all relations, create inteface at fortune teller to review stats and modifiers 
        }

        [Serializable]
        public class Place
        {
            public Vector3 position;
            public string buildingGuid;
        }

        [Serializable]
        public class Curse
        {
            public string guid;
            public int imageIdx;
            public int prophesyIdx;
        }
    }
}
