using Assets.Scripts.Helpers.ListView;
using TMPro;
using UnityEngine;

public class CurseCardListItem : MonoBehaviour, IListItem
{
    public CurseCard card;

    public TextMeshProUGUI curseName;
    public Transform imageParent;
    public GameObject details;

    public void FillItem(object data)
    {
        card = (CurseCard)data;

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
        Instantiate(details, transform.parent.parent)
            .GetComponent<CurseDetailWindow>().Init(card);
        Debug.Log("Card selected: " + card.curseName);
    }

    public void OnUnselected()
    {
        Debug.Log("Card unslected: " + card.curseName);
    }
}
