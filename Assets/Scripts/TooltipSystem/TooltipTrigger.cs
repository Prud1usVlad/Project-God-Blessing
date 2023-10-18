using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    [TextArea]
    public string content;
    public float delay = 0.5f;

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
        TooltipSystem.Show(content, header);
    }
}
