using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Helpers.Roguelike.EnumHandlers;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Models;
using Unity.VisualScripting;
using UnityEngine;


namespace Assets.Scripts.Roguelike.LevelGeneration.Logic
{
    public class Generator : MonoBehaviour
    {
        public LevelInputProperties LevelInputProperties = new LevelInputProperties();


        [Header("Room Probabilities")]
        /// <summary>
        /// Probability of couple doors in one room
        /// Accept values from 0.1 to 1
        /// </summary>
        [Range(0.1f, 1f)]
        public float RoomProbability = 0.5f;

        /// <summary>
        /// Probability of exit room apperiance
        /// Accept values from 0.1 to 1
        /// </summary>
        [Range(0.1f, 1f)]
        public float ExitRoomProbability = 0.1f;

        /// <summary>
        /// Probability of boss room apperiance
        /// Accept values from 0.1 to 1
        /// </summary>
        [Range(0.1f, 1f)]
        public float BossRoomProbability = 0.1f;

        /// <summary>
        /// Probability of trial room apperiance
        /// Accept values from 0.1 to 1
        /// </summary>
        [Range(0.1f, 1f)]
        public float TrialRoomProbability = 0.7f;

        /// <summary>
        /// Probability of treasure room apperiance
        /// Accept values from 0.1 to 1
        /// </summary>
        [Range(0.1f, 1f)]
        public float TreasuresRoomProbability = 0.3f;

        /// <summary>
        /// Probability of resource room apperiance
        /// Accept values from 0.1 to 1
        /// </summary>
        [Range(0.1f, 1f)]
        public float ResourcesRoomProbability = 0.4f;

        [Header("Rooms Limits")]
        /// <summary>
        /// Limit of exit room on map
        /// Accept values from 0 to 1
        /// </summary>
        [Range(0f, 1f)]
        public float ExitRoomLimit = 0.03f;

        /// <summary>
        /// Limit of boss room on map
        /// Accept values from 0 to 1
        /// </summary>
        [Range(0f, 1f)]
        public float BossRoomLimit = 0.1f;

        /// <summary>
        /// Limit of trial room on map
        /// Accept values from 0 to 1
        /// </summary>
        [Range(0f, 1f)]
        public float TrialRoomLimit = 0.5f;

        /// <summary>
        /// Limit of resources room on map
        /// Accept values from 0 to 1
        /// </summary>
        [Range(0f, 1f)]
        public float ResourcesRoomLimit = 0.175f;

        /// <summary>
        /// Limit of treasures room on map
        /// Accept values from 0 to 1
        /// </summary>
        [Range(0f, 1f)]
        public float TreasuresRoomLimit = 0.15f;


        /// <summary>
        /// NonSerialized field with reference to the start room
        /// </summary>
        [NonSerialized]
        public Room StartRoom;

        private List<KeyValuePair<int, int>> roomsCoordinates = new List<KeyValuePair<int, int>>();

        private ControlInputHandler controlInputHandler;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        public void Start()
        {
            controlInputHandler = gameObject.transform.GetComponent<ControlInputHandler>();

            int generationNumber = 1;

            Action generate = delegate ()
            {
                roomsCoordinates.Clear();

                int mapSize = SizeHandler.GetSize(LevelInputProperties.Size);

                StartRoom = new Room()
                {
                    RoomType = RoomType.Spawn,
                    DepthOfRoomFromStart = 0,
                    RoomEntranceDoor = Direction.None
                };

                roomsCoordinates.Add(new KeyValuePair<int, int>(0, 0));

                int remains = roomsMapGenerate(StartRoom, mapSize, roomsCoordinates[0]);

                roomsFilling();

                Debug.Log(String.Format(
                    "Defined: {0}, Remains: {1}, Rooms calculated in main room: {2}, Generation number: {3}",
                    mapSize,
                    remains,
                    StartRoom.TotalAmountOfChieldRooms,
                    generationNumber));

                if(remains > 0)
                {
                    Debug.LogError($"Remains: {remains}");
                }

                generationNumber++;
            };

            generate.Invoke();


#if UNITY_EDITOR
            controlInputHandler.ControlActionList += delegate ()
            {
                if (Input.GetKeyUp(KeyCode.Backspace))
                {
                    generate.Invoke();
                }
            };
#endif
        }

        private int roomsMapGenerate(Room currentRoom, int currentRoomLeftAmount, KeyValuePair<int, int> currentCoordinates)
        {
            if (currentRoom == null)
            {
                return 0;
            }

            if (currentRoomLeftAmount == 0)
            {
                return 0;
            }

            if (currentRoomLeftAmount < 0)
            {
                throw new ArgumentException("Rooms amount bigger than map size");
            }

            bool
                isAnyWayUsed = false,
                isRightRoom = false,
                isLeftRoom = false,
                isTopRoom = false,
                isBottomRoom = false;

            float currentChance = RoomProbability;
            int roomAmount = 0;

            //Right room
            if (RandomHelper.GetResultOfChanceWheel(currentChance)
                && !roomsCoordinates.Any(x =>
                    x.Key == currentCoordinates.Key + 1 && x.Value == currentCoordinates.Value))
            {
                isAnyWayUsed = true;
                isRightRoom = true;

                currentRoomLeftAmount--;
                roomAmount++;
                currentRoom.RightRoom = new Room()
                {
                    LeftRoom = currentRoom,
                    DepthOfRoomFromStart = currentRoom.DepthOfRoomFromStart + 1,
                    RoomEntranceDoor = Direction.L,
                    RoomType = RoomType.Empty
                };
                roomsCoordinates.Add(new KeyValuePair<int, int>(currentCoordinates.Key + 1, currentCoordinates.Value));

                currentChance -= currentChance / 2;
                currentChance = currentChance < .1f ? .1f : currentChance;
            }
            else
            {
                currentChance += currentChance / 2;
                currentChance = currentChance > 1.0f ? 1.0f : currentChance;
            }

            //Left room
            if (RandomHelper.GetResultOfChanceWheel(currentChance)
                && currentRoomLeftAmount > 0
                && !roomsCoordinates.Any(x =>
                    x.Key == currentCoordinates.Key - 1 && x.Value == currentCoordinates.Value))
            {
                isAnyWayUsed = true;
                isLeftRoom = true;

                currentRoomLeftAmount--;
                roomAmount++;
                currentRoom.LeftRoom = new Room()
                {
                    RightRoom = currentRoom,
                    DepthOfRoomFromStart = currentRoom.DepthOfRoomFromStart + 1,
                    RoomEntranceDoor = Direction.R,
                    RoomType = RoomType.Empty
                };
                roomsCoordinates.Add(new KeyValuePair<int, int>(currentCoordinates.Key - 1, currentCoordinates.Value));

                currentChance -= currentChance / 2;
                currentChance = currentChance < .1f ? .1f : currentChance;
            }
            else
            {
                currentChance += currentChance / 2;
                currentChance = currentChance > 1.0f ? 1.0f : currentChance;
            }

            //Top room
            if (RandomHelper.GetResultOfChanceWheel(currentChance)
                && currentRoomLeftAmount > 0
                && !roomsCoordinates.Any(x =>
                    x.Key == currentCoordinates.Key && x.Value == currentCoordinates.Value + 1))
            {
                isAnyWayUsed = true;
                isTopRoom = true;

                currentRoomLeftAmount--;
                roomAmount++;
                currentRoom.TopRoom = new Room()
                {
                    BottomRoom = currentRoom,
                    DepthOfRoomFromStart = currentRoom.DepthOfRoomFromStart + 1,
                    RoomEntranceDoor = Direction.B,
                    RoomType = RoomType.Empty
                };
                roomsCoordinates.Add(new KeyValuePair<int, int>(currentCoordinates.Key, currentCoordinates.Value + 1));

                currentChance -= currentChance / 2;
                currentChance = currentChance < .1f ? .1f : currentChance;
            }
            else
            {
                currentChance += currentChance / 2;
                currentChance = currentChance > 1.0f ? 1.0f : currentChance;
            }

            //Bottom room
            if (RandomHelper.GetResultOfChanceWheel(currentChance)
                && currentRoomLeftAmount > 0
                && !roomsCoordinates.Any(x =>
                    x.Key == currentCoordinates.Key && x.Value == currentCoordinates.Value - 1))
            {
                isAnyWayUsed = true;
                isBottomRoom = true;

                currentRoomLeftAmount--;
                roomAmount++;
                currentRoom.BottomRoom = new Room()
                {
                    TopRoom = currentRoom,
                    DepthOfRoomFromStart = currentRoom.DepthOfRoomFromStart + 1,
                    RoomEntranceDoor = Direction.T,
                    RoomType = RoomType.Empty
                };
                roomsCoordinates.Add(new KeyValuePair<int, int>(currentCoordinates.Key, currentCoordinates.Value - 1));
            }

            if (!isAnyWayUsed)
            {
                switch (getSideOfNextRoom(currentCoordinates))
                {
                    case 1:
                        isRightRoom = true;
                        currentRoom.RightRoom = new Room()
                        {
                            LeftRoom = currentRoom,
                            DepthOfRoomFromStart = currentRoom.DepthOfRoomFromStart + 1,
                            RoomEntranceDoor = Direction.L,
                            RoomType = RoomType.Empty
                        };
                        roomsCoordinates.Add(new KeyValuePair<int, int>(currentCoordinates.Key + 1, currentCoordinates.Value));
                        currentRoomLeftAmount--;
                        break;
                    case 2:
                        isLeftRoom = true;
                        currentRoom.LeftRoom = new Room()
                        {
                            RightRoom = currentRoom,
                            DepthOfRoomFromStart = currentRoom.DepthOfRoomFromStart + 1,
                            RoomEntranceDoor = Direction.R,
                            RoomType = RoomType.Empty
                        };
                        roomsCoordinates.Add(new KeyValuePair<int, int>(currentCoordinates.Key - 1, currentCoordinates.Value));
                        currentRoomLeftAmount--;
                        break;
                    case 3:
                        isTopRoom = true;
                        currentRoom.TopRoom = new Room()
                        {
                            BottomRoom = currentRoom,
                            DepthOfRoomFromStart = currentRoom.DepthOfRoomFromStart + 1,
                            RoomEntranceDoor = Direction.B,
                            RoomType = RoomType.Empty
                        };
                        roomsCoordinates.Add(new KeyValuePair<int, int>(currentCoordinates.Key, currentCoordinates.Value + 1));
                        currentRoomLeftAmount--;
                        break;
                    case 4:
                        isBottomRoom = true;
                        currentRoom.BottomRoom = new Room()
                        {
                            TopRoom = currentRoom,
                            DepthOfRoomFromStart = currentRoom.DepthOfRoomFromStart + 1,
                            RoomEntranceDoor = Direction.T,
                            RoomType = RoomType.Empty
                        };
                        roomsCoordinates.Add(new KeyValuePair<int, int>(currentCoordinates.Key, currentCoordinates.Value - 1));
                        currentRoomLeftAmount--;
                        break;
                    default:
                        return currentRoomLeftAmount;
                }

                roomAmount = 1;
            }


            // Debug.Log(String.Format("Depth {0} Rooms Left: {1}, Rooms amount: {2}",
            //     currentRoom.DepthOfRoomFromStart,
            //     currentRoomLeftAmount,
            //     roomAmount
            // ));

            int roomRemainAmount = 0;
            int roomsAmoutForThisSide = 0;
            bool oddCheck = false;
            if (isRightRoom)
            {
                roomsAmoutForThisSide = currentRoomLeftAmount / roomAmount;

                if (currentRoomLeftAmount % roomAmount != 0 && currentRoomLeftAmount > 0 && !oddCheck)
                {
                    oddCheck = true;
                    roomsAmoutForThisSide++;
                }

                roomRemainAmount = roomsMapGenerate(currentRoom.RightRoom,
                    roomsAmoutForThisSide,
                    new KeyValuePair<int, int>(currentCoordinates.Key + 1, currentCoordinates.Value));
            }

            if (isLeftRoom)
            {
                roomsAmoutForThisSide = currentRoomLeftAmount / roomAmount + roomRemainAmount;


                if (currentRoomLeftAmount % roomAmount != 0 && currentRoomLeftAmount > 0 && !oddCheck)
                {
                    oddCheck = true;
                    roomsAmoutForThisSide++;
                }

                roomRemainAmount = roomsMapGenerate(currentRoom.LeftRoom,
                    roomsAmoutForThisSide,
                    new KeyValuePair<int, int>(currentCoordinates.Key - 1, currentCoordinates.Value));
            }

            if (isTopRoom)
            {
                roomsAmoutForThisSide = currentRoomLeftAmount / roomAmount + roomRemainAmount;


                if (currentRoomLeftAmount % roomAmount != 0 && currentRoomLeftAmount > 0 && !oddCheck)
                {
                    oddCheck = true;
                    roomsAmoutForThisSide++;
                }

                roomRemainAmount = roomsMapGenerate(currentRoom.TopRoom,
                    roomsAmoutForThisSide,
                    new KeyValuePair<int, int>(currentCoordinates.Key, currentCoordinates.Value + 1));
            }

            if (isBottomRoom)
            {
                roomsAmoutForThisSide = currentRoomLeftAmount / roomAmount + roomRemainAmount;


                if (currentRoomLeftAmount % roomAmount != 0 && currentRoomLeftAmount > 0 && !oddCheck)
                {
                    oddCheck = true;
                    roomsAmoutForThisSide++;
                }

                roomRemainAmount = roomsMapGenerate(currentRoom.BottomRoom,
                    roomsAmoutForThisSide,
                    new KeyValuePair<int, int>(currentCoordinates.Key, currentCoordinates.Value - 1));
            }

            // Debug.Log(String.Format("Depth {0} Rooms Left: {1}, Rooms amount: {2}, Rooms remain: {3}",
            //     currentRoom.DepthOfRoomFromStart,
            //     currentRoomLeftAmount,
            //     roomAmount,
            //     roomRemainAmount
            // ));

            currentRoom.TotalAmountOfChieldRooms = currentRoom.TotalAmountOfChieldRooms
                + currentRoomLeftAmount - roomRemainAmount + roomAmount;

            if (roomRemainAmount != 0)
            {
                //Debug.Log($"At room ({currentCoordinates.Key}, {currentCoordinates.Value}), remain {roomRemainAmount} rooms");

                Room previousRoom = null;
                KeyValuePair<int, int> previousCoordinates = new KeyValuePair<int, int>();

                switch (currentRoom.RoomEntranceDoor)
                {
                    case Direction.T:
                        previousRoom = currentRoom.TopRoom;
                        previousCoordinates = new KeyValuePair<int, int>(currentCoordinates.Key, currentCoordinates.Value + 1);
                        break;
                    case Direction.B:
                        previousRoom = currentRoom.BottomRoom;
                        previousCoordinates = new KeyValuePair<int, int>(currentCoordinates.Key, currentCoordinates.Value - 1);
                        break;
                    case Direction.R:
                        previousRoom = currentRoom.RightRoom;
                        previousCoordinates = new KeyValuePair<int, int>(currentCoordinates.Key + 1, currentCoordinates.Value);
                        break;
                    case Direction.L:
                        previousRoom = currentRoom.LeftRoom;
                        previousCoordinates = new KeyValuePair<int, int>(currentCoordinates.Key - 1, currentCoordinates.Value);
                        break;
                    case Direction.None:
                        Debug.LogError($"At spawn room also left some rooms: {roomRemainAmount}");
                        break;
                    default:
                        Debug.LogError("Unknown entrance door direction");
                        break;
                }

                if (previousRoom != null)
                {
                    roomRemainAmount = roomsMapGenerate(previousRoom, roomRemainAmount, previousCoordinates);
                }
            }

            return roomRemainAmount;
        }

        private int getSideOfNextRoom(KeyValuePair<int, int> currentCoordinates)
        {
            bool isntRight = roomsCoordinates.Any(x =>
                                x.Key == currentCoordinates.Key + 1 && x.Value == currentCoordinates.Value),
                instLeft = roomsCoordinates.Any(x =>
                                x.Key == currentCoordinates.Key - 1 && x.Value == currentCoordinates.Value),
                isntTop = roomsCoordinates.Any(x =>
                                x.Key == currentCoordinates.Key && x.Value == currentCoordinates.Value + 1),
                instBottom = roomsCoordinates.Any(x =>
                                x.Key == currentCoordinates.Key && x.Value == currentCoordinates.Value - 1);

            List<int> sides = new List<int>();

            if (!isntRight) sides.Add(1);
            if (!instLeft) sides.Add(2);
            if (!isntTop) sides.Add(3);
            if (!instBottom) sides.Add(4);

            if (sides.Count == 0)
            {
                return -1;
            }

            return sides[(int)UnityEngine.Random.Range(0f, sides.Count)];
        }

        private void roomsFilling()
        {
            int exitRoomsAmount = 0,
                bossRoomsAmount = 0,
                trialRoomsAmount = 0,
                resourcesRoomsAmount = 0,
                treasuresRoomsAmount = 0;

            int maxExitRoomsAmount = (int)Math.Ceiling(StartRoom.TotalAmountOfChieldRooms * ExitRoomLimit),
                maxBossRoomsAmount = (int)Math.Ceiling(StartRoom.TotalAmountOfChieldRooms * BossRoomLimit),
                maxTrialRoomsAmount = (int)Math.Ceiling(StartRoom.TotalAmountOfChieldRooms * TrialRoomLimit),
                maxResourcesRoomsAmount = (int)Math.Ceiling(StartRoom.TotalAmountOfChieldRooms * ResourcesRoomLimit),
                maxTreasuresRoomsAmount = (int)Math.Ceiling(StartRoom.TotalAmountOfChieldRooms * TreasuresRoomLimit);

            Func<Room, float, bool> exitRoomCondition = (room, probability) =>
                exitRoomsAmount < maxExitRoomsAmount
                    ? RandomHelper.GetResultOfChanceWheel(probability)
                    : false;

            Func<Room, float, bool, float> exitRoomProbabilityCondition = (room, probability, isAppeared) =>
            {
                if (exitRoomsAmount >= maxExitRoomsAmount)
                {
                    return 0f;
                }

                float newProbability = probability + probability / 20 * (isAppeared ? -1 : 1);
                newProbability = newProbability < 0.1f ? 0.1f : newProbability > 1 ? 1 : newProbability;

                return newProbability;
            };

            Func<Room, float, bool> bossRoomCondition = (room, probability) =>
                bossRoomsAmount < maxBossRoomsAmount
                    ? RandomHelper.GetResultOfChanceWheel(probability)
                    : false;

            Func<Room, float, bool, float> bossRoomProbabilityCondition = (room, probability, isAppeared) =>
            {
                float newProbability = probability;
                if (bossRoomsAmount > 0)
                {
                    newProbability += 0.1f * (isAppeared ? -3 : 1);
                }
                else
                {
                    newProbability += probability / 3 * (isAppeared ? -3 : 1);
                }

                newProbability = newProbability < 0.1f ? 0.1f : newProbability > 1 ? 1 : newProbability;

                return newProbability;
            };

            Func<Room, float, bool> trialRoomCondition = (room, probability) =>
                trialRoomsAmount < maxTrialRoomsAmount
                    ? RandomHelper.GetResultOfChanceWheel(probability)
                    : false;

            Func<Room, float, bool, float> trialRoomProbabilityCondition = (room, probability, isAppeared) =>
            {
                if (trialRoomsAmount >= maxTrialRoomsAmount)
                {
                    return 0f;
                }

                float newProbability = probability + probability / 2 * (isAppeared ? -1 : 1);

                newProbability = newProbability < 0.1f ? 0.1f : newProbability > 1 ? 1 : newProbability;

                return newProbability;
            };

            Func<Room, float, bool> resourcesRoomCondition = (room, probability) =>
                resourcesRoomsAmount < maxResourcesRoomsAmount
                    ? RandomHelper.GetResultOfChanceWheel(probability)
                    : false;

            Func<Room, float, bool, float> resourcesRoomProbabilityCondition = (room, probability, isAppeared) =>
            {
                if (resourcesRoomsAmount >= maxResourcesRoomsAmount)
                {
                    return 0f;
                }

                float newProbability = probability + probability / 4 * (isAppeared ? -1 : 1);

                newProbability = newProbability < 0.1f ? 0.1f : newProbability > 1 ? 1 : newProbability;

                return newProbability;
            };

            Func<Room, float, bool> treasuresRoomCondition = (room, probability) =>
                treasuresRoomsAmount < maxTreasuresRoomsAmount
                    ? RandomHelper.GetResultOfChanceWheel(probability)
                    : false;

            Func<Room, float, bool, float> treasuresRoomProbabilityCondition = (room, probability, isAppeared) =>
            {
                if (treasuresRoomsAmount >= maxTreasuresRoomsAmount)
                {
                    return 0f;
                }

                float newProbability = probability + probability / 7 * (isAppeared ? -3.5f : 1);

                newProbability = newProbability < 0.1f ? 0.1f : newProbability > 1 ? 1 : newProbability;

                return newProbability;
            };

            roomsLayerFiller(StartRoom, exitRoomCondition, RoomType.Exit,
                ExitRoomProbability, exitRoomProbabilityCondition, ref exitRoomsAmount);

            while (bossRoomsAmount == 0)
            {
                roomsLayerFiller(StartRoom, bossRoomCondition, RoomType.Boss,
                    BossRoomProbability, bossRoomProbabilityCondition, ref bossRoomsAmount);
            }

            roomsLayerFiller(StartRoom, treasuresRoomCondition, RoomType.Treasures,
                TreasuresRoomProbability, treasuresRoomProbabilityCondition, ref treasuresRoomsAmount);

            roomsLayerFiller(StartRoom, trialRoomCondition, RoomType.Trial,
                TrialRoomProbability, trialRoomProbabilityCondition, ref trialRoomsAmount);

            roomsLayerFiller(StartRoom, resourcesRoomCondition, RoomType.Resources,
                ResourcesRoomProbability, resourcesRoomProbabilityCondition, ref resourcesRoomsAmount);
        }

        private void roomsLayerFiller(
            Room currentRoom,
            Func<Room, float, bool> condition,
            RoomType type,
            float currenrtProbability,
            Func<Room, float, bool, float> probabilityCondition,
            ref int roomAmount)
        {

            if (currentRoom == null)
            {
                return;
            }

            if (currentRoom.RoomType == RoomType.Empty
                && condition(currentRoom, currenrtProbability))
            {
                currentRoom.RoomType = type;
                roomAmount++;
                currenrtProbability = probabilityCondition(currentRoom, currenrtProbability, true);
            }
            else
            {
                currenrtProbability = probabilityCondition(currentRoom, currenrtProbability, false);
            }

            if (currentRoom.RoomEntranceDoor != Direction.T)
            {
                roomsLayerFiller(currentRoom.TopRoom, condition, type,
                currenrtProbability, probabilityCondition, ref roomAmount);
            }

            if (currentRoom.RoomEntranceDoor != Direction.R)
            {
                roomsLayerFiller(currentRoom.RightRoom, condition, type,
                currenrtProbability, probabilityCondition, ref roomAmount);
            }

            if (currentRoom.RoomEntranceDoor != Direction.B)
            {
                roomsLayerFiller(currentRoom.BottomRoom, condition, type,
                currenrtProbability, probabilityCondition, ref roomAmount);
            }

            if (currentRoom.RoomEntranceDoor != Direction.L)
            {
                roomsLayerFiller(currentRoom.LeftRoom, condition, type,
                currenrtProbability, probabilityCondition, ref roomAmount);
            }
        }
    }
}