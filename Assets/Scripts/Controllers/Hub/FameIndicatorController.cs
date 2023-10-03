using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FameIndicatorController : MonoBehaviour
{
    public FameTranslation fameTranslation;

    public TextMeshProUGUI fameLevelText;
    public TextMeshProUGUI fameDescriptionText;
    public TextMeshProUGUI famePoints;
    public Slider fameSlider;

    private void OnEnable()
    {
        float currPoints = fameTranslation.currentPoints;
        float nededPoints = fameTranslation.GetPointsForNextLevel() + currPoints;

        fameLevelText.SetText("Fame level: " 
            + fameTranslation.currentFameLevel.name);
        fameDescriptionText.SetText(
            fameTranslation.currentFameLevel.description);
        famePoints.SetText($"{currPoints}/{nededPoints}");

        float sliderPart = 1f / (fameTranslation.registry.count - 1f);
        float sliderFillAmount = sliderPart * fameTranslation.currentLevelIdx;
        float lowerVal = fameTranslation.currentFameLevel.points;

        sliderFillAmount += ((currPoints - lowerVal) * sliderPart) / (nededPoints - lowerVal);

        fameSlider.value = sliderFillAmount;

        famePoints.gameObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        famePoints.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        famePoints.gameObject.SetActive(false);
    }
}
