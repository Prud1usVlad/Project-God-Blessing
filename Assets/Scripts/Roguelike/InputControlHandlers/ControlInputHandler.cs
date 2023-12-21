using System;
using UnityEngine;

namespace Assets.Scripts.Helpers.Roguelike
{
    public class ControlInputHandler : MonoBehaviour
    {
        /// <summary>
        /// List of event which control all of appointed actions
        /// </summary>
        public event Action ControlActionList;
        
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            ControlActionList?.Invoke();
        }
    }
}