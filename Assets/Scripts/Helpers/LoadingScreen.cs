using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    [CreateAssetMenu(menuName = "ScriptableObjects/LoadingScreen")]
    public class LoadingScreen : ScriptableObject
    {
        public GameObject screenPrefab;
        public GameObject loadingScreen;

        public void Show()
        {
            loadingScreen = Instantiate(screenPrefab);
            DontDestroyOnLoad(loadingScreen);

        }

        public void Hide() 
        {
            if (loadingScreen != null)
                Destroy(loadingScreen);
        }
    }
}
