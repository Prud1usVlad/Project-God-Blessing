using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class Extensions
    {
        public static List<T> GetRandomItems<T>(this List<T> list, int amount)
        {
            List<T> tmpList = new List<T>(list);
            List<T> newList = new List<T>();

            while (newList.Count < amount && tmpList.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, tmpList.Count);
                newList.Add(tmpList[index]);
                tmpList.RemoveAt(index);
            }

            return newList;
        }
    }
}
