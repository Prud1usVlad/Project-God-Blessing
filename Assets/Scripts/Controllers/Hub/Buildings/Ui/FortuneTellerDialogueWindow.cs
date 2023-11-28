using Assets.Scripts.Helpers;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FortuneTellerDialogueWindow : DialogueBox
{
    public LiesTranslation liesTranslation;
    public GameProgress gameProgress;
    public Slider liesSlider;

    public ListViewController cardsList;
    public GameObject noCursesText;

    public GameObject playerStatsPrefab;

    public override bool InitDialogue()
    {
        header = "Fortune Teller";
        body = "Tells fortunes";

        var inited = base.InitDialogue();
        
        cardsList.InitView(gameProgress.curses.Cast<object>().ToList());

        if (inited)
        {
            UpdateView();
        }

        return inited;
    }

    public void UpdateView()
    {
        liesSlider.value = (float)liesTranslation.currentPoints / (float)liesTranslation.pointsCap;
        cardsList.RefreshList(gameProgress.curses.Cast<object>().ToList());

        if (gameProgress.curses.Count() == 0)
            noCursesText.SetActive(true);
        else
            noCursesText.SetActive(false);
    }

    public void CloseWindow()
    {
        modalManager.DialogueClose();
    }

    public void ShowPlayerStats()
    {
        var dialogue = Instantiate(playerStatsPrefab, transform)
            .GetComponent<PlayerStatsWindow>();

        modalManager.DialogueOpen(dialogue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            liesTranslation.AddPoints(10);
            UpdateView();   
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            liesTranslation.RemovePoints(10);
            UpdateView();
        }
    }
}
