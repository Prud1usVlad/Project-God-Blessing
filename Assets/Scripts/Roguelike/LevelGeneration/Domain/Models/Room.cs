using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;

namespace Assets.Scripts.Roguelike.LevelGeneration.Domain.Models
{
    [System.Serializable]
    public class Room
    {
        public Room LeftRoom;

        public Room TopRoom;

        public Room RightRoom;

        public Room BottomRoom;

        public RoomType RoomType;

        public int DepthOfRoomFromStart;

        public int TotalAmountOfChieldRooms;

        public Direction RoomEntranceDoor;

        public RoomForm GetRoomForm()
        {
            int roomsAmount = 0;

            roomsAmount += TopRoom == null ? 0 : 1000;
            roomsAmount += RightRoom == null ? 0 : 100;
            roomsAmount += BottomRoom == null ? 0 : 10;
            roomsAmount += LeftRoom == null ? 0 : 1;

            return (RoomForm)roomsAmount;
        }
    }
}