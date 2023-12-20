using System;
using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Helpers.Roguelike.Minimap;
using UnityEngine;

namespace Assets.Scripts.Roguelike.StateHandlers
{
    public class MinimapStateHandler : MonoBehaviour
    {
        private void Start()
        {
            PlayerStateHelper.Instance.PlayerStateChangeActions += delegate ()
            {
                switch (PlayerStateHelper.Instance.PlayerState)
                {
                    case PlayerState.Dead:
                        gameObject.SetActive(false);
                        break;
                    case PlayerState.InGame:
                        gameObject.SetActive(true);
                        gameObject.GetComponent<Minimap>().enabled = true;
                        break;
                    case PlayerState.Pause:
                        gameObject.GetComponent<Minimap>().enabled = false;
                        break;
                    default:
                        throw new Exception("Unhandled player state");
                }
            };
        }
    }
}