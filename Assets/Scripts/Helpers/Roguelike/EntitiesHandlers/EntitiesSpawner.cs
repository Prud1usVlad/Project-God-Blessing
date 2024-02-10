using System;
using System.Collections.Generic;
using Assets.Scripts.Roguelike.Entities.Enemy;
using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.Entities.Resource;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Models;
using UnityEngine;

namespace Assets.Scripts.Helpers.Roguelike.EntitiesHandlers
{
    public class EntitiesSpawner : MonoBehaviour
    {
        public Room Room;

        public GameObject Player;
        public LevelRoomDecorator LevelRoomDecorator;

        public EntitiesConcentrator EntitiesConcentrator;

        public void SpawnEntities()
        {
            if (Room.RoomType.Equals(RoomType.Empty))
            {
                return;
            }
            if (Room.RoomType.Equals(RoomType.Spawn))
            {
                Player.transform.position = transform.position;
                return;
            }

            Transform destinationSpawnGroup = null;

            foreach (Transform child in transform)
            {
                if (child.tag.Equals(TagHelper.SpawnPointTags.SpawnPointGroupTag))
                {
                    destinationSpawnGroup = child;

                    break;
                }
            }

            if (destinationSpawnGroup == null)
            {
                throw new Exception("Any spawn points does not existed");
            }

            string targetGroupTag, targetSingleTag;
            List<GameObject> targetEntities;

            switch (Room.RoomType)
            {
                case RoomType.Boss:
                    targetGroupTag = TagHelper.SpawnPointTags.BossSpawnPointGroupTag;
                    targetSingleTag = TagHelper.SpawnPointTags.BossSpawnPointTag;
                    targetEntities = EntitiesConcentrator.Bosses;
                    break;
                case RoomType.Trial:
                    targetGroupTag = TagHelper.SpawnPointTags.EnemySpawnPointGroupTag;
                    targetSingleTag = TagHelper.SpawnPointTags.EnemySpawnPointTag;
                    targetEntities = EntitiesConcentrator.Enemies;
                    break;
                case RoomType.Treasures:
                    targetGroupTag = TagHelper.SpawnPointTags.TreasuresSpawnPointGroupTag;
                    targetSingleTag = TagHelper.SpawnPointTags.TreasuresSpawnPointTag;
                    targetEntities = EntitiesConcentrator.TreasureObjects;
                    break;
                case RoomType.Resources:
                    targetGroupTag = TagHelper.SpawnPointTags.ResourcesSpawnPointGroupTag;
                    targetSingleTag = TagHelper.SpawnPointTags.ResourcesSpawnPointTag;
                    targetEntities = EntitiesConcentrator.ResourceObjects;
                    break;
                case RoomType.Exit:
                    targetGroupTag = TagHelper.SpawnPointTags.ExitSpawnPointGroupTag;
                    targetSingleTag = TagHelper.SpawnPointTags.ExitSpawnPointTag;
                    targetEntities = EntitiesConcentrator.ExitObjects;
                    break;
                default:
                    throw new Exception("Unhandled room type");
            }

            Transform targetSpawnGroup = null;

            foreach (Transform child in destinationSpawnGroup)
            {
                if (child.tag.Equals(targetGroupTag))
                {
                    targetSpawnGroup = child;

                    break;
                }
            }

            if (targetSpawnGroup == null)
            {
                throw new Exception("Any target spawn point group does not existed");
            }

            foreach (Transform child in targetSpawnGroup)
            {
                if (child.tag.Equals(targetSingleTag))
                {
                    GameObject instance = Instantiate(
                        targetEntities[(int)UnityEngine.Random.Range(0f, targetEntities.Count)],
                        child.position,
                        Quaternion.identity);
                    instance.transform.parent = gameObject.transform;
                    instance.GetComponent<IEntitySpawnDecorator>().SetSpawnSetting(default);

                    if (Room.RoomType.Equals(RoomType.Boss) || Room.RoomType.Equals(RoomType.Trial))
                    {
                        LevelRoomDecorator.RegisterEnemy(instance);

                    }

                    if (Room.RoomType.Equals(RoomType.Boss))
                    {
                        EnemyDecorator enemyDecorator = instance.GetComponent<EnemyDecorator>();

                        enemyDecorator.EnemyDeath = delegate ()
                        {
                            Instantiate(
                                EntitiesConcentrator.ExitObjects[(int)UnityEngine.Random
                                    .Range(0f, EntitiesConcentrator.ExitObjects.Count)],
                                child.position,
                                Quaternion.identity,
                                transform.parent);
                        }
                        + enemyDecorator.EnemyDeath;
                    }
                }
            }
        }
    }
}