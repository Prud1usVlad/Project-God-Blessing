using System;
using UnityEngine;

namespace Assets.Scripts.Helpers.Roguelike
{
    public class RestartGameHelper : MonoBehaviour
    {
        private static RestartGameHelper _instance;
        public Action Restart;

        public static RestartGameHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<RestartGameHelper>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = GameObject.FindGameObjectWithTag(TagHelper.HandlersObjectTag) 
                            ?? new GameObject("HandlersObject") {tag = TagHelper.HandlersObjectTag};
                        _instance = singletonObject.AddComponent<RestartGameHelper>();
                    }
                }

                return _instance;
            }
        }

        private RestartGameHelper(){}
    }
}