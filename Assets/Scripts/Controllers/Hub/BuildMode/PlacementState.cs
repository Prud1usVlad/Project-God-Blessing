using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controllers.Hub.BuildMode
{
    public class PlacementState : IBuildState
    {
        private Building building;
        string guid;
        BuildingRegistry registry;
        ObjectPlacer objectPlacer;
        PreviewSystem previewSystem;
        List<BuildingPlace> buildingPlaces;

        public PlacementState(string guid, 
            BuildingRegistry registry, ObjectPlacer objectPlacer,
                PreviewSystem previewSystem, List<BuildingPlace> places)
        {
            this.guid = guid;
            //this.grid = grid;
            this.previewSystem = previewSystem;
            this.registry = registry;
            buildingPlaces = places;
            //this.floorData = floorData;
            //this.furnitureData = furnitureData;
            this.objectPlacer = objectPlacer;
            //this.soundFeedback = soundFeedback;

            building = registry.Find(guid) as Building;

            if (building is not null)
            {
                previewSystem.StartShowingPlacementPreview(building.prefab);
            }
            else
                throw new System.Exception($"No object with ID {guid}");

        }

        public void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3 position)
        {

            bool placementValidity = CheckPlacementValidity(position);
            if (placementValidity == false)
            {
                //soundFeedback.PlaySound(SoundType.wrongPlacement);
                return;
            }
            //soundFeedback.PlaySound(SoundType.Place);
            int index = objectPlacer.PlaceObject(building.prefab, position);
            var place = buildingPlaces.Find(p => p.transform.position == position);
            place.building = building;

            previewSystem.UpdatePosition(position, false);
        }

        private bool CheckPlacementValidity(Vector3 position)
        {
            var place = buildingPlaces.Find(p => p.transform.position == position);
            if (place is null) return false;
            else return place.building is null;
        }

        public void UpdateState(Vector3 position)
        {
            bool placementValidity = CheckPlacementValidity(position);

            previewSystem.UpdatePosition(position, placementValidity);
        }
    }
}
