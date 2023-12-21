using Assets.Scripts.ScriptableObjects.Hub;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject savesMenu;
    public GameObject newCharacterMenu;
    public ModalManager modalManager;

    public void OnLoadGame()
    {
        var dialogue = Instantiate(savesMenu, transform)
            .GetComponent<SavesMenuWindow>();

        modalManager.DialogueOpen(dialogue);
    }

    public void OnNewGame()
    {
        var dialogue = Instantiate(newCharacterMenu, transform)
            .GetComponent<NewCharacterWindow>();
        
        modalManager.DialogueOpen(dialogue);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            modalManager.DialogueClose();
        }
    }

}
