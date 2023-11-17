using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject savesMenu;
    public GameObject newCharacterMenu;

    public void OnLoadGame()
    {
        Instantiate(savesMenu, transform);
    }

    public void OnNewGame()
    {
        Instantiate(newCharacterMenu, transform);
    }

}
