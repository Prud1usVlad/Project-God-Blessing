using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdditiveSliderWidget : MonoBehaviour
{
    public Color accentColor;

    public Slider sliderA;
    public Slider sliderB;
    public TextMeshProUGUI percents;

    public void UpdateView(float a, float b, Color color, 
        bool showPercents = false, float max = 1, float min = 0)
    {
        sliderA.maxValue = max;
        sliderB.maxValue = max;
        sliderA.minValue = min;
        sliderB.minValue = min;

        sliderA.value = a;
        sliderB.value = b;

        if (showPercents)
        {
            float percent = ((b - a) * 100f) / max;

            percents.gameObject.SetActive(true);
            var text = percent > 0 ? $"+{percent}%" : $"-{percent}%";
            percents.SetText(text);
        }
        else
        {
            percents.gameObject.SetActive(false);
        }

        AddColors(color);
    }


    private void AddColors(Color color)
    {
        sliderA.fillRect.GetComponent<Image>().color = color;
        sliderB.fillRect.GetComponent<Image>().color = accentColor;
    }
}