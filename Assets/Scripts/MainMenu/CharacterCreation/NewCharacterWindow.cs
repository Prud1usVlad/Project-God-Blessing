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
    public GameObject modeSelection;
    public int amountOfStoryCards = 6;

    [Header("By hand creation")]
    public GameObject handCreationPanel;
    public Image avatar;
    public TMP_InputField characterName;
    public TextMeshProUGUI freeStatPoints;
    public List<StatSelector> statSelectors;
    public ListViewController allAvatarsList;
    public GameObject allAvatarsView;
    public Button createButton;

    public List<string> randomNames;
    public List<Sprite> randomAvatars;

    [Header("Story based creation")]
    public GameObject storyBasedPanel;
    public List<CharacterStoryCard> storyCards;
    public ListViewController storyCardsView;
    public GameObject cardInfoPanel;
    public Image cardAvatar;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardStory;
    public List<StatSelector> cardStatSelectors;
    public GameObject modifyCardButton;
    public GameObject statsPanel;
    public GameObject statsRevealInfo;

    public CharacterStoryCard selectedCard;

    [Header("Utils")]
    public GameProgress progress;
    public SaveSystem saveSystem;
    public LoadingScreen loadingScreen;

    public void Awake()
    {
        SetRandomName();
        avatar.sprite = randomAvatars[Random.Range(0, randomAvatars.Count)];
        StatSelector.freePoints = statPoints;
        allAvatarsList.selectionChanged += OnAvatarSelect;
        storyCardsView.selectionChanged += StoryCardSelect;

        modeSelection.gameObject.SetActive(true);
        storyBasedPanel.SetActive(false);
        handCreationPanel.SetActive(false);
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

    public void InitByHand()
    {
        modeSelection.gameObject.SetActive(false);
        storyBasedPanel.gameObject.SetActive(false);
        handCreationPanel.gameObject.SetActive(true);
    }

    public void InitByStory()
    {
        modeSelection.gameObject.SetActive(false);
        handCreationPanel.gameObject.SetActive(false);
        storyBasedPanel.gameObject.SetActive(true);

        ShuffleStoryCards();
    }

    public void CreateCharacter()
    {
        loadingScreen.Show();

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

    public void StoryCardSelect()
    {
        var card = storyCardsView.Selected as CharacterStoryCard;
        StoryCardSelect(card);
    }

    public void StoryCardSelect(CharacterStoryCard card)
    {
        if (card != null) 
        {
            selectedCard = card;
            cardInfoPanel.SetActive(true);
            modifyCardButton.SetActive(true);
            statsPanel.SetActive(false);
            statsRevealInfo.SetActive(true);

            cardAvatar.sprite = card.avatar;
            cardName.SetText(card.characterName);   
            cardStory.SetText(card.story);

            foreach (var selector in cardStatSelectors)
            {
                selector.UpdateView((int)card.defaultStats
                    .Find(s => s.name == selector.statName).baseValue);
            }
        }
    }

    public void HideCardInfo()
    {
        cardInfoPanel.gameObject.SetActive(false);
    }

    public void CreateCharacterFromCard()
    {
        var saveFile = new SaveFile();
        saveFile.day = 1;
        saveFile.characterName = selectedCard.characterName;
        saveFile.type = "Initial";
        saveFile.date = Converters.DateTimeToSaveDate(System.DateTime.Now);
        saveFile.avatarSprite = selectedCard.avatar;
        saveFile.defaultStats = selectedCard.defaultStats;

        saveSystem.SaveData(saveFile, "InitialSave " + saveFile.date + ".json");

        SceneManager.LoadScene(0);
    }

    public void ModifyCard()
    {
        HideCardInfo();
        InitByHand();

        avatar.sprite = selectedCard.avatar;
        characterName.text = selectedCard.characterName;

        foreach (var selector in statSelectors)
            selector.SetValue(0);

        foreach (var stat in selectedCard.defaultStats)
        {
            statSelectors
                .Find(s => s.statName == stat.name)
                .SetValue((int)stat.baseValue);
        }
    }
    
    public void RevealStatsInfo()
    {
        statsPanel.SetActive(true);
        statsRevealInfo.SetActive(false);
    }

    public void ShuffleStoryCards()
    {
        storyCardsView.InitView(storyCards
            .GetRandomItems(amountOfStoryCards).Cast<object>().ToList());
    }

    public void PickRandomCard()
    {
        StoryCardSelect(storyCards.GetRandomItems(1).First());
        modifyCardButton.gameObject.SetActive(false);
    }
}
