using Assets.Scripts.Helpers.ListView;
using System;
using TMPro;
using UnityEngine;

public class CurseCardListItem : MonoBehaviour, IListItem
{
    public CurseCard card;

    public TextMeshProUGUI curseName;
    public Transform imageParent;
    public GameObject details;

    public Action Selection { get; set; }

    public void FillItem(object data)
    {
        card = data as CurseCard;

        curseName?.SetText(card?.curseName);
        Instantiate(card.image, imageParent);

    }

    public bool HasData(object data)
    {
        if (data is CurseCard)
        {
            return (data as CurseCard) == card;
        }
        else return false;
    }

    public void OnSelected()
    {
        
    }

    public void OnSelecting()
    {
        Selection?.Invoke();
        var dialogue = Instantiate(details, transform.parent.parent)
            .GetComponent<CurseDetailWindow>();
        dialogue.InitData(card);
        dialogue.modalManager.DialogueOpen(dialogue);
    }
}
