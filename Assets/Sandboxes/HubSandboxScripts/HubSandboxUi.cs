using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.ScriptableObjects.Hub;
using Assets.Scripts.Stats;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubSandboxUi : MonoBehaviour
{
    public RuntimeHubUiData runtimeHubUiData;

    private void Awake()
    {
        Debug.Log(runtimeHubUiData.name);   
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {

            SceneManager.LoadScene(0);
        }
    }
}
