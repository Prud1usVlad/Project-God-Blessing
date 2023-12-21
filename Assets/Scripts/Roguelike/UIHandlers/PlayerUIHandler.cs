using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers.Roguelike;
using UnityEngine;

public class PlayerUIHandler : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
        
    }

    public void Restart()
    {
        Player.GetComponent<PlayerInputController>().OnRestart();
        RestartGameHelper.Instance.Restart.Invoke();
    }
}
