using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.Hub.BuildMode
{
    public class RemovingState : IBuildState
    {
        private float cashBack = 0.3f;

        private string guid;
        private BuildingRegistry registry;
        private ObjectPlacer objectPlacer;
        private PreviewSystem previewSystem;
        private ResourceContainer resourceContainer;
        
        private List<BuildingPlace> buildingPlaces;

        public RemovingState(ObjectPlacer objectPlacer,
            PreviewSystem previewSystem, List<BuildingPlace> places,
            ResourceContainer resourceContainer)
        {
            this.previewSystem = previewSystem;
            buildingPlaces = places;
            this.objectPlacer = objectPlacer;
            this.resourceContainer = resourceContainer;
            //this.soundFeedback = soundFeedback;

            previewSystem.StartShowingRemovePreview();

        }

        public void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3 position, 
            Action<Action<Vector3>, Vector3> confirm = null)
        {
            BuildingPlace place = buildingPlaces.Find(p 
                => p.gameObject.transform.position == position);

            if (place is null || place.Building is null)
            {
                Debug.Log("Cant delete");
                //soundFeedback.PlaySound(SoundType.wrongPlacement);
            }
            else
            {
                if (confirm == null)
                    ActionBase(position);
                else
                    confirm.Invoke((v) => ActionBase(position), position);
            }
            
        }

        public void UpdateState(Vector3 position)
        {
            bool validity = CheckIfSelectionIsValid(position);
            previewSystem.UpdatePosition(position, validity);
        }

        private bool CheckIfSelectionIsValid(Vector3 position)
        {
            var place = buildingPlaces.Find(p => p.transform.position == position);
            if (place is null) return true;
            else return place.Building is not null;
        }

        private void ActionBase(Vector3 position)
        {
            BuildingPlace place = buildingPlaces.Find(p
                => p.gameObject.transform.position == position);
            var building = place.Building;

            //soundFeedback.PlaySound(SoundType.Remove);
            objectPlacer.RemoveObjectAt(place);

            objectPlacer.RemoveObjectAt(place);
            previewSystem.UpdatePosition(position, CheckIfSelectionIsValid(position));

            foreach (var res in building.price.resources)
            {
                resourceContainer.GetResource(res.name)
                    .GainResource(Mathf.RoundToInt(res.amount * cashBack),
                    TransactionType.Cashback);
            }
        }
    }
}
