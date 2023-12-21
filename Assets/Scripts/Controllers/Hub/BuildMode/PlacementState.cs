using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Controllers.Hub.BuildMode
{
    public class PlacementState : IBuildState
    {
        private Building building;
        private string guid;
        private BuildingRegistry registry;
        private ObjectPlacer objectPlacer;
        private PreviewSystem previewSystem;
        private List<BuildingPlace> buildingPlaces;
        private ResourceContainer resourceContainer;

        public PlacementState(string guid, 
            BuildingRegistry registry, ObjectPlacer objectPlacer,
            PreviewSystem previewSystem, List<BuildingPlace> places,
            ResourceContainer resourceContainer)
        {
            this.guid = guid;
            this.previewSystem = previewSystem;
            this.registry = registry;
            buildingPlaces = places;
            this.objectPlacer = objectPlacer;
            this.resourceContainer = resourceContainer;
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

        public void OnAction(Vector3 position, 
            Action<Action<Vector3>, Vector3> confirm = null)
        {
            bool placementValidity = CheckPlacementValidity(position);
            if (placementValidity == false)
            {
                //soundFeedback.PlaySound(SoundType.wrongPlacement);
                return;
            }

            if (confirm == null)
                ActionBase(position);
            else
                confirm.Invoke(ActionBase, position);

        }

        public void UpdateState(Vector3 position)
        {
            bool placementValidity = CheckPlacementValidity(position);

            previewSystem.UpdatePosition(position, placementValidity);
        }

        private bool CheckPlacementValidity(Vector3 position)
        {
            var place = buildingPlaces.Find(p => p.transform.position == position);
            if (place is null) return false;
            else return place.Building is null;
        }

        private void ActionBase(Vector3 position)
        {
            //soundFeedback.PlaySound(SoundType.Place);
            var place = buildingPlaces.Find(p => p.transform.position == position);
            int index = objectPlacer.PlaceObject(building, place);

            previewSystem.UpdatePosition(position, false);

            resourceContainer.Spend(building.price);
        }
    }
}
