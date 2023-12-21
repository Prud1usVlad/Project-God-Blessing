using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Models;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Roguelike
{
    public class WallDecorator : MonoBehaviour
    {
        [Header("Doors")]
        public GameObject LeftDoor;
        public GameObject TopDoor;
        public GameObject RightDoor;
        public GameObject BottomDoor;

        private Room _room;

        public Room Room
        {
            get
            {
                return _room;
            }
            set
            {
                _room = value;

                if (_room.BottomRoom == null)
                {
                    Destroy(BottomDoor);
                }
                if (_room.LeftRoom == null)
                {
                    Destroy(LeftDoor);
                }
                if (_room.TopRoom == null)
                {
                    Destroy(TopDoor);
                }
                if (_room.RightRoom == null)
                {
                    Destroy(RightDoor);
                }
            }
        }

    }
}