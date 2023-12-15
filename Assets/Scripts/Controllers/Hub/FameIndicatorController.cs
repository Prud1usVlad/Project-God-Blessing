using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FameIndicatorController : MonoBehaviour
{
    private float currPoints;
    private float nededPoints;

    public FameTranslation fameTranslation;

    public TextMeshProUGUI fameLevelText;
    public TextMeshProUGUI fameDescriptionText;
    public TooltipTrigger trigger;
    public Slider fameSlider;

    private void OnEnable()
    {
        currPoints = fameTranslation.currentPoints;
        nededPoints = fameTranslation.GetPointsForNextLevel() + currPoints;

        fameLevelText.SetText("Fame level: " 
            + fameTranslation.currentFameLevel.name);
        fameDescriptionText.SetText(
            fameTranslation.currentFameLevel.description);

        trigger.Init($"Current points: {currPoints}/{nededPoints}", "Fame level");

        float sliderPart = 1f / (fameTranslation.registry.count - 1f);
        float sliderFillAmount = sliderPart * fameTranslation.currentLevelIdx;
        float lowerVal = fameTranslation.currentFameLevel.points;

        sliderFillAmount += ((currPoints - lowerVal) * sliderPart) / (nededPoints - lowerVal);

        fameSlider.value = sliderFillAmount;
    }
}
