using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.Hub.BuildMode
{
    public class RemovingState : IBuildState
    {
        private Building building;
        string guid;
        BuildingRegistry registry;
        ObjectPlacer objectPlacer;
        PreviewSystem previewSystem;
        List<BuildingPlace> buildingPlaces;

        public RemovingState(ObjectPlacer objectPlacer,
                PreviewSystem previewSystem, List<BuildingPlace> places)
        {
            this.previewSystem = previewSystem;
            buildingPlaces = places;
            this.objectPlacer = objectPlacer;
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

            if (place is null || place.building is null)
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
            else return place.building is not null;
        }

        private void ActionBase(Vector3 position)
        {
            BuildingPlace place = buildingPlaces.Find(p
                => p.gameObject.transform.position == position);

            //soundFeedback.PlaySound(SoundType.Remove);
            objectPlacer.RemoveObjectAt(place);

            objectPlacer.RemoveObjectAt(place);
            previewSystem.UpdatePosition(position, CheckIfSelectionIsValid(position));
        }
    }
}
