using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Roguelike.Entities.Enemy;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
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
        private bool _isRoomCleared;
        public WallDecorator WallDecorator;

        private List<GameObject> _enemyList = new List<GameObject>();

        public void RegisterEnemy(GameObject enemy)
        {
            _enemyList.Add(enemy);
            enemy.GetComponent<EnemyDecorator>().EnemyDeath += delegate ()
            {
                RemoveEnemy(enemy);
            };

            IsRoomCleared = !_enemyList.Any();
        }

        public void RemoveEnemy(GameObject enemy)
        {
            _enemyList.Remove(enemy);

            IsRoomCleared = !_enemyList.Any();
        }

        public bool IsRoomCleared
        {
            get
            {
                return _isRoomCleared;
            }

            set
            {
                _isRoomCleared = value;
                WallDecorator.RoomStatus(value);
            }
        }


        public Room Room
        {
            get
            {
                return _room;
            }
            set
            {
                _room = value;

                if (_room.RoomType.Equals(RoomType.Boss) || _room.RoomType.Equals(RoomType.Trial))
                {
                    _isRoomCleared = false;
                }
                else
                {
                    _isRoomCleared = true;
                }
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