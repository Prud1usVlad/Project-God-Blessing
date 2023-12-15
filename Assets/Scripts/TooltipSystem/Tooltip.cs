using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private RectTransform rectTransform;

    public Canvas canvas;
    public TextMeshProUGUI header;
    public TextMeshProUGUI content;
    public LayoutElement layoutElement;

    public int characterWrapLimit;

    private void Awake()
    {
         rectTransform = GetComponent<RectTransform>();
    }

    public void InitView(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            this.header.gameObject.SetActive(false);
        }
        else
        {
            this.header.gameObject.SetActive(true);
            this.header.SetText(header);
        }

        this.content.SetText(content);

        var hLen = this.header.text.Length;
        var cLen = this.content.text.Length;

        layoutElement.enabled = (hLen >= characterWrapLimit || cLen >= characterWrapLimit);
    }

    private void Update()
    {
        var pos = Input.mousePosition * canvas.scaleFactor;
        float xPivot = pos.x / Screen.width;
        float yPivot = pos.y / Screen.height;

        xPivot = xPivot > 0.5 ? 1 : 0;
        yPivot = yPivot > 0.5 ? 1 : 0;

        transform.position = pos;
        rectTransform.pivot = new Vector2(xPivot, yPivot);
    }
}
