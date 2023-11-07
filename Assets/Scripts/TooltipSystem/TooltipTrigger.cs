using Assets.Scripts.TooltipSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    [TextArea]
    public string content;
    public float delay = 0.5f;

    public TooltipDataProvider dataProvider;
    public string providerTag = null;

    public void Init(string content, string header = "")
    {
        this.content = content;
        this.header = header;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(ShowRoutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        TooltipSystem.Hide();
    }

    private IEnumerator ShowRoutine()
    {
        yield return new WaitForSeconds(delay);

        if (dataProvider is not null)
        {
            header = dataProvider.GetHeader(providerTag);
            content = dataProvider.GetContent(providerTag);
        }

        TooltipSystem.Show(content, header);
    }
}
