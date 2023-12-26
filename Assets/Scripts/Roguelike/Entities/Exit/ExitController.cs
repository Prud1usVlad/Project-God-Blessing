
using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.Roguelike;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    private GameObject _leftLevelScreen;
    public void LeftLevel()
    {
        _leftLevelScreen.SetActive(true);
        PlayerStateHelper.Instance.PlayerState = PlayerState.Pause;
    }

    private void Start()
    {
        GameObject playerUI = GameObject.FindWithTag(TagHelper.UITags.PlayerUITag);
        foreach (Transform child in playerUI.transform)
        {
            if (child.CompareTag(TagHelper.UITags.Screens.LeftLevelScreenTag))
            {
                _leftLevelScreen = child.gameObject;
                _leftLevelScreen.SetActive(false);
                break;
            }
        }

        if (_leftLevelScreen == null)
        {
            throw new Exception("Exit screen find error");
        }
    }
}