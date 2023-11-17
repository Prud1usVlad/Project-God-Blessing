using Assets.Scripts.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavesMenuWindow : MonoBehaviour
{
    public ListViewController savesView;

    public SaveSystem saveSystem;
    public GameProgress gameProgress;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    public void Awake()
    {
        UpdateView();
    }

    public void UpdateView()
    {
        var files = saveSystem.LoadAll<SaveFile>();

        savesView.allowSelection = false;
        savesView.InitView(files.Cast<object>().ToList());
    }

    public void OnDeleteSaveFile(string fileName)
    {
        saveSystem.DeleteFile(fileName);
        UpdateView();
    }

    public void OnLoadSaveFile(string fileName)
    {
        gameProgress.preferedSaveFile = fileName;
        SceneManager.LoadScene(0);
    }
}
