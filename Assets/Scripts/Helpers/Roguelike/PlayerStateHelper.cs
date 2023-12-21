using System;
using UnityEngine;

namespace Assets.Scripts.Helpers.Roguelike
{
    public class PlayerStateHelper : MonoBehaviour
    {
        private static PlayerStateHelper _instance;
        private PlayerState _playerState;
        public Action PlayerStateChangeActions;
        public PlayerState PlayerState
        {
            get
            {
                return _playerState;
            }

            set
            {
                _playerState = value;
                PlayerStateChangeActions.Invoke();
            }
        }



        public static PlayerStateHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<PlayerStateHelper>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = GameObject.FindGameObjectWithTag(TagHelper.HandlersObjectTag)
                            ?? new GameObject("HandlersObject") { tag = TagHelper.HandlersObjectTag };
                        _instance = singletonObject.AddComponent<PlayerStateHelper>();
                    }
                }

                return _instance;
            }
        }

        private PlayerStateHelper() { }
    }
}