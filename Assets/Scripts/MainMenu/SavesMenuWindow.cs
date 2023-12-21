using Assets.Scripts.Helpers;
using Assets.Scripts.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavesMenuWindow : DialogueBox
{
    public ListViewController savesView;

    public SaveSystem saveSystem;
    public GameProgress gameProgress;
    public LoadingScreen loadingScreen;

    public override bool InitDialogue()
    {
        var inited = base.InitDialogue();

        if (inited)
        {
            UpdateView();
        }

        return inited;
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
        loadingScreen.Show();
        gameProgress.preferedSaveFile = fileName;
        SceneManager.LoadScene(0);
    }

    public void OnClose()
    {
        modalManager.DialogueClose();
    }
}
