using System;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Models;
using UnityEngine;

namespace Assets.Scripts.Roguelike.LevelGeneration.Domain.Models
{
    public class LevelRoomDecorator : MonoBehaviour
    {
        private Room _room;

        private bool _isCurrentRoom;

        public GameObject RightRoom = null;
        public GameObject BottomRoom = null;
        public GameObject LeftRoom = null;
        public GameObject TopRoom = null;

        public Room Room
        {
            get
            {
                return _room;
            }
            set
            {
                _room = value;
            }
        }

        public bool IsCurrentRoom
        {
            get
            {
                return _isCurrentRoom;
            }
            set
            {
                gameObject.SetActive(value);

                _isCurrentRoom = value;
            }
        }
    }
}