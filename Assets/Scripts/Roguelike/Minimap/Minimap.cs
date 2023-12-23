using System;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Models;
using Assets.Scripts.Roguelike.LevelGeneration.Logic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

namespace Assets.Scripts.Helpers.Roguelike.Minimap
{
    public class Minimap : MonoBehaviour
    {
        [Header("Room prefabs")]
        public GameObject TRBL;
        public GameObject Locked;
        public GameObject TRB;
        public GameObject TRL;
        public GameObject TBL;
        public GameObject RBL;
        public GameObject BL;
        public GameObject TL;
        public GameObject TR;
        public GameObject RB;
        public GameObject TB;
        public GameObject RL;
        public GameObject T;
        public GameObject R;
        public GameObject B;
        public GameObject L;

        [Header("Room icons")]

        public GameObject BossIcon;
        public GameObject TrialIcon;
        public GameObject ExitIcon;
        public GameObject ResourcesIcon;
        public GameObject TreasuresIcon;


        [Header("Game objects")]
        public GameObject Level;

        private Generator _generator;
        private ControlInputHandler _controlInputHandler;
        private GameObject _currentRoom;
        private bool _isGenerated;
        private Action _mapGenerartingPipeline;

        private void Start()
        {
            _generator = Level.GetComponent<Generator>();
            _controlInputHandler = Level.GetComponent<ControlInputHandler>();

            RoomMovementController.Instance.MoveTopDoorFunc = moveTopDoor + RoomMovementController.Instance.MoveTopDoorFunc;
            RoomMovementController.Instance.MoveRightDoorFunc = MoveRightDoor + RoomMovementController.Instance.MoveRightDoorFunc;
            RoomMovementController.Instance.MoveDownDoorFunc = moveDownDoor + RoomMovementController.Instance.MoveDownDoorFunc;
            RoomMovementController.Instance.MoveLeftDoorFunc = moveLeftDoor + RoomMovementController.Instance.MoveLeftDoorFunc ;

            RestartGameHelper.Instance.Restart += delegate ()
            {
                _isGenerated = false;
                _mapGenerartingPipeline.Invoke();
            };

            _mapGenerartingPipeline += mapFilling;

#if UNITY_EDITOR
            _mapGenerartingPipeline += delegate ()
            {
                if (Input.GetKeyUp(KeyCode.Backspace))
                {
                    _isGenerated = false;
                }
            };
        }
#endif

        private void Update()
        {
            _mapGenerartingPipeline.Invoke();
        }

        private void mapFilling()
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
                        if (!child.tag.Equals(TagHelper.MinimapPlayerIconTag)
                         && !child.tag.Equals(TagHelper.UITag))
                        {
                            Destroy(child.gameObject, 0f);
                        }
                    }
                    //_controlInputHandler.ControlActionList -= mapFilling;
                    _currentRoom = minimapSpawn(_generator.StartRoom, transform);

                    if (_currentRoom == null)
                    {
                        throw new Exception("None of spawn room is found");
                    }

                    RoomDecorator room = _currentRoom.GetComponent<RoomDecorator>();

                    room.IsCurrentRoom = true;
                }
                else
                {
                    Debug.Log("Empty level");
                }
            }
        }

        private GameObject minimapSpawn(Room currentRoom, Transform spawnPoint)
        {
            if (currentRoom == null)
            {
                return null;
            }

            GameObject room;
            RoomForm type = currentRoom.GetRoomForm();

            switch (type)
            {
                case RoomForm.TRBL:
                    room = TRBL;
                    break;
                case RoomForm.Locked:
                    room = Locked;
                    break;
                case RoomForm.TRB:
                    room = TRB;
                    break;
                case RoomForm.TRL:
                    room = TRL;
                    break;
                case RoomForm.TBL:
                    room = TBL;
                    break;
                case RoomForm.RBL:
                    room = RBL;
                    break;
                case RoomForm.BL:
                    room = BL;
                    break;
                case RoomForm.TL:
                    room = TL;
                    break;
                case RoomForm.TR:
                    room = TR;
                    break;
                case RoomForm.RB:
                    room = RB;
                    break;
                case RoomForm.TB:
                    room = TB;
                    break;
                case RoomForm.RL:
                    room = RL;
                    break;
                case RoomForm.T:
                    room = T;
                    break;
                case RoomForm.R:
                    room = R;
                    break;
                case RoomForm.B:
                    room = B;
                    break;
                case RoomForm.L:
                    room = L;
                    break;
                default:
                    throw new ArgumentException($"Unexpected room form: {type}");
            }

            GameObject instance = Instantiate(room, spawnPoint.position, Quaternion.identity);
            instance.transform.parent = gameObject.transform;

            Transform instanceTransform = instance.transform;

            instance.AddComponent<RoomDecorator>();
            instance.GetComponent<RoomDecorator>().Room = currentRoom;

            Transform legendIconTransform = null;

            foreach (Transform child in instanceTransform)
            {
                if (child.tag.Equals(TagHelper.SpawnPointTags.RoomSpawnPointTag))
                {
                    legendIconTransform = child;
                    break;
                }
            }

            switch (currentRoom.RoomType)
            {
                case RoomType.Boss:
                    GameObject bossIconInstance = Instantiate(BossIcon, instanceTransform.position, Quaternion.identity);
                    bossIconInstance.transform.parent = instanceTransform;
                    break;
                case RoomType.Trial:
                    GameObject trialIconInstance = Instantiate(TrialIcon, instanceTransform.position, Quaternion.identity);
                    trialIconInstance.transform.parent = instanceTransform;
                    break;
                case RoomType.Treasures:
                    GameObject treasuresIconInstance = Instantiate(TreasuresIcon, instanceTransform.position, Quaternion.identity);
                    treasuresIconInstance.transform.parent = instanceTransform;
                    break;
                case RoomType.Resources:
                    GameObject resourcesIconInstance = Instantiate(ResourcesIcon, instanceTransform.position, Quaternion.identity);
                    resourcesIconInstance.transform.parent = instanceTransform;
                    break;
                case RoomType.Exit:
                    GameObject exitIconInstance = Instantiate(ExitIcon, instanceTransform.position, Quaternion.identity);
                    exitIconInstance.transform.parent = instanceTransform;
                    break;
            }

            foreach (Transform child in instanceTransform)
            {
                if (child.tag.Equals(TagHelper.RoomSpawnPointTag))
                {
                    if (child.localPosition.x < 0 && currentRoom.RoomEntranceDoor != Direction.L)
                    {
                        minimapSpawn(currentRoom.LeftRoom, child);
                    }
                    if (child.localPosition.x > 0 && currentRoom.RoomEntranceDoor != Direction.R)
                    {
                        minimapSpawn(currentRoom.RightRoom, child);
                    }
                    if (child.localPosition.y < 0 && currentRoom.RoomEntranceDoor != Direction.B)
                    {
                        minimapSpawn(currentRoom.BottomRoom, child);
                    }
                    if (child.localPosition.y > 0 && currentRoom.RoomEntranceDoor != Direction.T)
                    {
                        minimapSpawn(currentRoom.TopRoom, child);
                    }
                }
            }

            return instance;
        }

        private void moveTopDoor()
        {
            RoomDecorator room = _currentRoom.GetComponent<RoomDecorator>();
            if (room.Room.TopRoom != null)
            {
                room.IsCurrentRoom = false;

                Transform topTransform = null;

                foreach (Transform children in _currentRoom.transform)
                {
                    if (children.localPosition.y > 0 && children.tag.Equals(TagHelper.RoomSpawnPointTag))
                    {
                        topTransform = children;
                        break;
                    }
                }

                if (topTransform == null)
                {
                    throw new Exception("None of top spawn point is found");
                }

                foreach (Transform children in transform)
                {
                    if (children.tag.Equals(TagHelper.MinimapRoomTag))
                    {
                        if (children.position.Equals(topTransform.position))
                        {
                            _currentRoom = children.gameObject;
                            break;
                        }
                    }
                }

                room = _currentRoom.GetComponent<RoomDecorator>();

                room.IsCurrentRoom = true;
                room.IsVisible = true;
            }
        }

        private void MoveRightDoor()
        {
            RoomDecorator room = _currentRoom.GetComponent<RoomDecorator>();
            if (room.Room.RightRoom != null)
            {
                room.IsCurrentRoom = false;

                Transform rightTransform = null;

                foreach (Transform children in _currentRoom.transform)
                {
                    if (children.localPosition.x > 0 && children.tag.Equals(TagHelper.RoomSpawnPointTag))
                    {
                        rightTransform = children;
                        break;
                    }
                }

                if (rightTransform == null)
                {
                    throw new Exception("None of right spawn point is found");
                }

                foreach (Transform children in transform)
                {
                    if (children.tag.Equals(TagHelper.MinimapRoomTag))
                    {
                        if (children.position.Equals(rightTransform.position))
                        {
                            _currentRoom = children.gameObject;
                            break;
                        }
                    }
                }

                room = _currentRoom.GetComponent<RoomDecorator>();

                room.IsCurrentRoom = true;
                room.IsVisible = true;
            }
        }

        private void moveDownDoor()
        {
            RoomDecorator room = _currentRoom.GetComponent<RoomDecorator>();
            if (room.Room.BottomRoom != null)
            {
                room.IsCurrentRoom = false;

                Transform downTransform = null;

                foreach (Transform children in _currentRoom.transform)
                {
                    if (children.localPosition.y < 0 && children.tag.Equals(TagHelper.RoomSpawnPointTag))
                    {
                        downTransform = children;
                        break;
                    }
                }

                if (downTransform == null)
                {
                    throw new Exception("None of right spawn point is found");
                }

                foreach (Transform children in transform)
                {
                    if (children.tag.Equals(TagHelper.MinimapRoomTag))
                    {
                        if (children.position.Equals(downTransform.position))
                        {
                            _currentRoom = children.gameObject;
                            break;
                        }
                    }
                }

                room = _currentRoom.GetComponent<RoomDecorator>();

                room.IsCurrentRoom = true;
                room.IsVisible = true;
            }
        }

        private void moveLeftDoor()
        {
            RoomDecorator room = _currentRoom.GetComponent<RoomDecorator>();
            if (room.Room.LeftRoom != null)
            {
                room.IsCurrentRoom = false;

                Transform leftTransform = null;

                foreach (Transform children in _currentRoom.transform)
                {
                    if (children.localPosition.x < 0 && children.tag.Equals(TagHelper.RoomSpawnPointTag))
                    {
                        leftTransform = children;
                        break;
                    }
                }

                if (leftTransform == null)
                {
                    throw new Exception("None of right spawn point is found");
                }

                foreach (Transform children in transform)
                {
                    if (children.tag.Equals(TagHelper.MinimapRoomTag))
                    {
                        if (children.position.Equals(leftTransform.position))
                        {
                            _currentRoom = children.gameObject;
                            break;
                        }
                    }
                }

                room = _currentRoom.GetComponent<RoomDecorator>();

                room.IsCurrentRoom = true;
                room.IsVisible = true;
            }
        }
    }
}