using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Roguelike.LevelGeneration.Domain.Models
{
    public class RoomPartParams : MonoBehaviour
    {
        public RoomSize RoomSize;

        public List<LevelRoomType> RoomTypes;

        public List<EnvironmentType> EnvironmentTypes;

        public bool RoomSizesCheck(List<RoomSize> list)
        {
            return list.Any(x => x.Equals(RoomSize));
        }

        public bool LevelRoomTypesCheck(List<LevelRoomType> list)
        {
            return list.Any(x => RoomTypes.Contains(x) || x.Equals(LevelRoomType.Default));
        }

        public bool EnvironmentTypesCheck(List<EnvironmentType> list)
        {
            return list.Any(x => EnvironmentTypes.Contains(x) || x.Equals(EnvironmentType.Universal));
        }

        public bool RoomSizeCheck(RoomSize value)
        {
            return RoomSize.Equals(value);
        }

        public bool LevelRoomTypeCheck(RoomType value)
        {
            return RoomTypes.Any(x => x.ToString().Equals(value.ToString()))
                || RoomTypes.Contains(LevelRoomType.Default);
        }

        public bool EnvironmentTypeCheck(EnvironmentType value)
        {
            return EnvironmentTypes.Contains(value)
                || EnvironmentTypes.Contains(EnvironmentType.Universal);
        }
    }
}