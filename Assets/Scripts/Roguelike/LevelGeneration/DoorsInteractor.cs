using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.Roguelike.Minimap;
using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Models;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Roguelike
{
    public class DoorsInteractor : MonoBehaviour
    {
        public GameObject Player;

        public WallDecorator TopWallDecorator;
        public WallDecorator RightWallDecorator;
        public WallDecorator BottomWallDecorator;
        public WallDecorator LeftWallDecorator;

        public Room Room;

        public void MoveDownDoor()
        {
            if(BottomWallDecorator == null || !gameObject.activeInHierarchy) return;

            Transform spawnPoint = null;

            foreach (Transform child in BottomWallDecorator.TopDoor.transform)
            {
                if (child.tag.Equals(TagHelper.SpawnPointTags.PlayerSpawnPointTag))
                {
                    spawnPoint = child;
                    break;
                }
            }

            if (spawnPoint == null)
            {
                throw new Exception("Destination door does not have spawn point for player");
            }

            Player.transform.position = spawnPoint.position;
        }

        public void MoveLeftDoor()
        {
            if(LeftWallDecorator == null || !gameObject.activeInHierarchy) return;

            Transform spawnPoint = null;

            foreach (Transform child in LeftWallDecorator.RightDoor.transform)
            {
                if (child.tag.Equals(TagHelper.SpawnPointTags.PlayerSpawnPointTag))
                {
                    spawnPoint = child;
                    break;
                }
            }

            if (spawnPoint == null)
            {
                throw new Exception("Destination door does not have spawn point for player");
            }

            Player.transform.position = spawnPoint.position;
        }

        public void MoveTopDoor()
        {
            if(TopWallDecorator == null || !gameObject.activeInHierarchy) return;

            Transform spawnPoint = null;

            foreach (Transform child in TopWallDecorator.BottomDoor.transform)
            {
                if (child.tag.Equals(TagHelper.SpawnPointTags.PlayerSpawnPointTag))
                {
                    spawnPoint = child;
                    break;
                }
            }

            if (spawnPoint == null)
            {
                throw new Exception("Destination door does not have spawn point for player");
            }

            Player.transform.position = spawnPoint.position;
        }

        public void MoveRightDoor()
        {
            if(RightWallDecorator == null || !gameObject.activeInHierarchy) return;

            Transform spawnPoint = null;

            foreach (Transform child in RightWallDecorator.LeftDoor.transform)
            {
                if (child.tag.Equals(TagHelper.SpawnPointTags.PlayerSpawnPointTag))
                {
                    spawnPoint = child;
                    break;
                }
            }

            if (spawnPoint == null)
            {
                throw new Exception("Destination door does not have spawn point for player");
            }

            Player.transform.position = spawnPoint.position;
        }
    }
}