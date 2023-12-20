using System;
using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Helpers.Roguelike.Minimap;
using UnityEngine;

namespace Assets.Scripts.Roguelike.StateHandlers
{
    public class PlayerUIStateHandler : MonoBehaviour
    {
        public Transform DeathScreen;
        public GameObject Minimap;

        private void Start()
        {
            PlayerStateHelper.Instance.PlayerStateChangeActions += delegate ()
            {
                switch (PlayerStateHelper.Instance.PlayerState)
                {
                    case PlayerState.Dead:
                        DeathScreen.parent = transform.parent;
                        DeathScreen.gameObject.SetActive(true);
                        Minimap.GetComponent<Minimap>().enabled = false;
                        gameObject.SetActive(false);
                        break;
                    case PlayerState.InGame:
                        DeathScreen.parent = transform;
                        DeathScreen.gameObject.SetActive(false);
                        gameObject.SetActive(true);
                        Minimap.GetComponent<Minimap>().enabled = true;
                        break;
                    case PlayerState.Pause:
                        Minimap.GetComponent<Minimap>().enabled = false;
                        break;
                    default:
                        throw new Exception("Unhandled player state");
                }
            };
        }
    }
}