using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Helpers.Roguelike.EntitiesHandlers;
using Assets.Scripts.Helpers.Roguelike.Minimap;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Models;
using Assets.Scripts.Roguelike.LevelGeneration.Logic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Roguelike.LevelGeneration
{
    public class LevelRoomSpawner : MonoBehaviour
    {
        [Header("Wall prefabs")]

        public List<GameObject> Walls;

        [Header("Floor prefabs")]

        public List<GameObject> Floors;

        [Header("Game objects")]
        public GameObject Level;
        public GameObject Minimap;
        public GameObject Player;

        private Generator _generator;

        private GameObject _currentRoom;
        private Action _spawnPipeline;

        private Dictionary<GameObject, List<GameObject>> _floorWallTable;

        private EnvironmentType _currentLevelEnvironmentType;

        private EntitiesConcentrator _entitiesConcentrator;

        private Minimap _minimap;
        private bool _isGenerated = false;

        private sealed class FloorWallPair
        {
            public GameObject Floor;

            public GameObject Wall;
        }

        private void Start()
        {
            _generator = Level.GetComponent<Generator>();
            _entitiesConcentrator = Level.GetComponent<EntitiesConcentrator>();

            _minimap = Minimap.GetComponent<Minimap>();

            RoomMovementController.Instance.MoveDownDoorFunc = moveDownDoor + RoomMovementController.Instance.MoveDownDoorFunc;
            RoomMovementController.Instance.MoveRightDoorFunc = moveRightDoor + RoomMovementController.Instance.MoveRightDoorFunc;
            RoomMovementController.Instance.MoveTopDoorFunc = moveTopDoor + RoomMovementController.Instance.MoveTopDoorFunc;
            RoomMovementController.Instance.MoveLeftDoorFunc = moveLeftDoor + RoomMovementController.Instance.MoveLeftDoorFunc;

            if (!setFloorWallPairs())
            {
                throw new Exception("Any pair of wall and floor does not exist");
            }


            RestartGameHelper.Instance.Restart += delegate ()
            {
                _isGenerated = false;
                _spawnPipeline.Invoke();
            };

            _spawnPipeline += spawn;

#if UNITY_EDITOR
            _spawnPipeline += delegate ()
            {
                if (Input.GetKeyUp(KeyCode.Backspace))
                {
                    _isGenerated = false;
                }
            };
#endif
        }

        private void Update()
        {
            _spawnPipeline?.Invoke();
        }

        private bool setFloorWallPairs()
        {
            bool pairCheck = false;

            _floorWallTable = new Dictionary<GameObject, List<GameObject>>();

            foreach (GameObject floor in Floors)
            {
                RoomPartParams floorParams = floor.GetComponent<RoomPartParams>();
                List<GameObject> walls = new List<GameObject>();

                foreach (GameObject wall in Walls)
                {
                    RoomPartParams wallParams = wall.GetComponent<RoomPartParams>();
                    if (floorParams.RoomSizeCheck(wallParams.RoomSize)
                        && floorParams.LevelRoomTypesCheck(wallParams.RoomTypes)
                        && floorParams.EnvironmentTypesCheck(wallParams.EnvironmentTypes))
                    {
                        pairCheck = true;
                        walls.Add(wall);
                    }
                }

                _floorWallTable.Add(floor, walls);
            }

            return pairCheck;
        }

        private void spawn()
        {
            if (_isGenerated)
            {
                return;
            }
            if (_generator != null)
            {
                if (_generator.StartRoom != null)
                {
                    _isGenerated = true;

                    foreach (Transform child in transform)
                    {
                        Destroy(child.gameObject, 0f);
                    }

                    _currentLevelEnvironmentType = _generator.LevelInputProperties.EnvironmentType;

                    if (!_floorWallTable.Any(x => x.Key.GetComponent<RoomPartParams>().EnvironmentTypeCheck(_currentLevelEnvironmentType)))
                    {
                        throw new Exception("Any pair of wall and floor with the same environmental type does not exist");
                    }

                    _currentRoom = roomSpawn(_generator.StartRoom, Vector3.zero, null);

                    if (_currentRoom == null)
                    {
                        throw new Exception("None of spawn room is found");
                    }

                    LevelRoomDecorator room = _currentRoom.GetComponent<LevelRoomDecorator>();

                    room.IsCurrentRoom = true;

                    _currentRoom.transform.parent.GetComponent<NavMeshSurface>().BuildNavMesh();
                }
                else
                {
                    Debug.Log("Empty level");
                }
            }
        }

        private GameObject roomSpawn(Room currentRoom, Vector3 spawnPosition, GameObject entranceRoom)
        {
            if (currentRoom == null)
            {
                return null;
            }

            GameObject instance = new GameObject($"{currentRoom.RoomType}_{currentRoom.DepthOfRoomFromStart}_{currentRoom.GetRoomForm()}");
            instance.transform.position = transform.position;
            instance.transform.rotation = Quaternion.identity;

            instance.transform.parent = gameObject.transform;

            FloorWallPair floorWallPair = getFloorWallPair(currentRoom);

            GameObject floorInstance = Instantiate(floorWallPair.Floor, instance.transform.position, Quaternion.identity);
            GameObject wallInstance = Instantiate(floorWallPair.Wall, instance.transform.position, Quaternion.identity);

            wallInstance.transform.parent = instance.transform;
            floorInstance.transform.parent = instance.transform;

            floorInstance.AddComponent<EntitiesSpawner>();

            EntitiesSpawner spawner = floorInstance.GetComponent<EntitiesSpawner>();
            spawner.Room = currentRoom;
            spawner.EntitiesConcentrator = _entitiesConcentrator;
            spawner.Player = Player;
            spawner.SpawnEntities();

            wallInstance.AddComponent<DoorsInteractor>();

            DoorsInteractor doorsInteractor = wallInstance.GetComponent<DoorsInteractor>();
            doorsInteractor.Room = currentRoom;
            doorsInteractor.Player = Player;

            RoomMovementController.Instance.MoveTopDoorFunc = doorsInteractor.MoveTopDoor + RoomMovementController.Instance.MoveTopDoorFunc;
            RoomMovementController.Instance.MoveRightDoorFunc = doorsInteractor.MoveRightDoor + RoomMovementController.Instance.MoveRightDoorFunc;
            RoomMovementController.Instance.MoveDownDoorFunc = doorsInteractor.MoveDownDoor + RoomMovementController.Instance.MoveDownDoorFunc;
            RoomMovementController.Instance.MoveLeftDoorFunc = doorsInteractor.MoveLeftDoor + RoomMovementController.Instance.MoveLeftDoorFunc;

            wallInstance.GetComponent<WallDecorator>().Room = currentRoom;

            instance.AddComponent<LevelRoomDecorator>();
            LevelRoomDecorator levelRoomDecorator = instance.GetComponent<LevelRoomDecorator>();
            levelRoomDecorator.Room = currentRoom;

            if (currentRoom.RoomEntranceDoor != Direction.L)
            {
                levelRoomDecorator.LeftRoom = roomSpawn(currentRoom.LeftRoom, spawnPosition, instance);
            }
            else
            {
                levelRoomDecorator.LeftRoom = entranceRoom;
            }
            if (currentRoom.RoomEntranceDoor != Direction.B)
            {
                levelRoomDecorator.BottomRoom = roomSpawn(currentRoom.BottomRoom, spawnPosition, instance);
            }
            else
            {
                levelRoomDecorator.BottomRoom = entranceRoom;
            }
            if (currentRoom.RoomEntranceDoor != Direction.R)
            {
                levelRoomDecorator.RightRoom = roomSpawn(currentRoom.RightRoom, spawnPosition, instance);
            }
            else
            {
                levelRoomDecorator.RightRoom = entranceRoom;
            }
            if (currentRoom.RoomEntranceDoor != Direction.T)
            {
                levelRoomDecorator.TopRoom = roomSpawn(currentRoom.TopRoom, spawnPosition, instance);
            }
            else
            {
                levelRoomDecorator.TopRoom = entranceRoom;
            }

            if (levelRoomDecorator.LeftRoom != null)
            {
                foreach (Transform child in levelRoomDecorator.LeftRoom.transform)
                {
                    if (child.tag.Equals(TagHelper.WallTag))
                    {
                        doorsInteractor.LeftWallDecorator = child.gameObject.GetComponent<WallDecorator>();
                        break;
                    }
                }
            }

            if (levelRoomDecorator.BottomRoom != null)
            {
                foreach (Transform child in levelRoomDecorator.BottomRoom.transform)
                {
                    if (child.tag.Equals(TagHelper.WallTag))
                    {
                        doorsInteractor.BottomWallDecorator = child.gameObject.GetComponent<WallDecorator>();
                        break;
                    }
                }
            }
            if (levelRoomDecorator.RightRoom != null)
            {
                foreach (Transform child in levelRoomDecorator.RightRoom.transform)
                {
                    if (child.tag.Equals(TagHelper.WallTag))
                    {
                        doorsInteractor.RightWallDecorator = child.gameObject.GetComponent<WallDecorator>();
                        break;
                    }
                }
            }
            if (levelRoomDecorator.TopRoom != null)
            {
                foreach (Transform child in levelRoomDecorator.TopRoom.transform)
                {
                    if (child.tag.Equals(TagHelper.WallTag))
                    {
                        doorsInteractor.TopWallDecorator = child.gameObject.GetComponent<WallDecorator>();
                        break;
                    }
                }
            }

            instance.SetActive(false);

            return instance;
        }

        private FloorWallPair getFloorWallPair(Room currentRoom)
        {
            List<FloorWallPair> floorWallPairs = new List<FloorWallPair>();

            foreach (var floorPairs in _floorWallTable)
            {
                RoomPartParams floorParams = floorPairs.Key.GetComponent<RoomPartParams>();
                if (!floorParams.LevelRoomTypeCheck(currentRoom.RoomType)
                        || !floorParams.EnvironmentTypeCheck(_currentLevelEnvironmentType))
                {
                    continue;
                }

                foreach (GameObject wall in floorPairs.Value)
                {
                    RoomPartParams wallParams = wall.GetComponent<RoomPartParams>();
                    if (wallParams.LevelRoomTypeCheck(currentRoom.RoomType)
                        && wallParams.EnvironmentTypeCheck(_currentLevelEnvironmentType))
                    {
                        floorWallPairs.Add(new FloorWallPair
                        {
                            Wall = wall,
                            Floor = floorPairs.Key
                        });
                    }
                }
            }

            if (!floorWallPairs.Any())
            {
                throw new Exception("Any pair of wall and floor with the same room parameters does not exist");
            }

            return floorWallPairs[(int)UnityEngine.Random.Range(0f, floorWallPairs.Count)];
        }

        public GameObject GetCurrentRoom()
        {
            return _currentRoom;
        }

        private void moveTopDoor()
        {
            LevelRoomDecorator room = _currentRoom.GetComponent<LevelRoomDecorator>();
            if (room.TopRoom != null)
            {
                room.IsCurrentRoom = false;

                _currentRoom = room.TopRoom;

                room = _currentRoom.GetComponent<LevelRoomDecorator>();
                room.IsCurrentRoom = true;
            }
        }

        private void moveRightDoor()
        {
            LevelRoomDecorator room = _currentRoom.GetComponent<LevelRoomDecorator>();
            if (room.RightRoom != null)
            {
                room.IsCurrentRoom = false;

                _currentRoom = room.RightRoom;

                room = _currentRoom.GetComponent<LevelRoomDecorator>();
                room.IsCurrentRoom = true;
            }
        }

        private void moveDownDoor()
        {
            LevelRoomDecorator room = _currentRoom.GetComponent<LevelRoomDecorator>();
            if (room.BottomRoom != null)
            {
                room.IsCurrentRoom = false;

                _currentRoom = room.BottomRoom;

                room = _currentRoom.GetComponent<LevelRoomDecorator>();
                room.IsCurrentRoom = true;
            }
        }

        private void moveLeftDoor()
        {
            LevelRoomDecorator room = _currentRoom.GetComponent<LevelRoomDecorator>();
            if (room.LeftRoom != null)
            {
                room.IsCurrentRoom = false;

                _currentRoom = room.LeftRoom;

                room = _currentRoom.GetComponent<LevelRoomDecorator>();
                room.IsCurrentRoom = true;
            }
        }
    }
}