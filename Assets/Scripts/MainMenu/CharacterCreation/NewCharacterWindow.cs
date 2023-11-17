using Assets.Scripts.Helpers;
using Assets.Scripts.MainMenu.CharacterCreation;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewCharacterWindow : MonoBehaviour
{
    public int statPoints;
    public Image avatar;
    public TMP_InputField characterName;
    public TextMeshProUGUI freeStatPoints;
    public List<StatSelector> statSelectors;
    public ListViewController allAvatarsList;
    public GameObject allAvatarsView;
    public Button createButton;

    public List<string> randomNames;
    public List<Sprite> randomAvatars;

    public GameProgress progress;
    public SaveSystem saveSystem;

    public void Awake()
    {
        SetRandomName();
        avatar.sprite = randomAvatars[Random.Range(0, randomAvatars.Count)];
        StatSelector.freePoints = statPoints;
        allAvatarsList.selectionChanged += OnAvatarSelect;
    }

    public void Update()
    {
        freeStatPoints.SetText(StatSelector.freePoints.ToString());

        if (createButton.interactable && StatSelector.freePoints != 0)
            createButton.interactable = false;
        else if (!createButton.interactable && StatSelector.freePoints == 0)
            createButton.interactable = true;

        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            Destroy(gameObject);
        }
    }

    public void CreateCharacter()
    {
        var saveFile = new SaveFile();
        saveFile.day = 1;
        saveFile.characterName = characterName.text;
        saveFile.type = "Initial";
        saveFile.date = Converters.DateTimeToSaveDate(System.DateTime.Now);
        saveFile.avatarSprite = avatar.sprite;
        saveFile.defaultStats = new();

        foreach (var selector in statSelectors) 
        {
            saveFile.defaultStats.Add(new Stat() { baseValue = selector.value, name = selector.statName });
        }

        saveSystem.SaveData(saveFile, "InitialSave " + saveFile.date + ".json");

        SceneManager.LoadScene(0);
    }

    public void ShowAllAvatars()
    {
        allAvatarsList.InitView(randomAvatars.Cast<object>().ToList(), avatar.sprite);
        allAvatarsView.gameObject.SetActive(true);
    }

    public void OnAvatarSelect()
    {
        avatar.sprite = allAvatarsList.Selected as Sprite;
        allAvatarsView.gameObject.SetActive(false);
    }

    public void SetRandomName()
    {
        characterName.SetTextWithoutNotify(randomNames[Random.Range(0, randomNames.Count)]);
    }
}
