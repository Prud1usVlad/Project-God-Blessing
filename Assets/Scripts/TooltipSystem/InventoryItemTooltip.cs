using Assets.Scripts.EquipmentSystem;
using TMPro;
using UnityEngine;

public class InventoryItemTooltip : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform rectTransform;
    public bool trackPosition = true;

    public TextMeshProUGUI header;
    public TextMeshProUGUI description;
    public TextMeshProUGUI nation;
    public TextMeshProUGUI type;

    public Transform modifiersPanel;
    public Transform sellPanel;
    public Transform deconstructPanel;

    public GameObject modifierWidget;
    public GameObject resourceAmountWidget;

    public void InitView(EquipmentItem item)
    {
        header.SetText($"{item.name} {item.level}Lvl");
        description.SetText(item.description);
        nation.SetText(System.Enum.GetName(typeof(NationName), item.nation));
        type.SetText(System.Enum.GetName(typeof(ItemType), item.type));

        DestroyAllChildren(modifiersPanel);
        DestroyAllChildren(sellPanel);
        DestroyAllChildren(deconstructPanel);

        foreach(var mod in item.modifiers)
        {
            var widget = Instantiate(modifierWidget, modifiersPanel)
                .GetComponent<ModifierWidget>();
            widget.UpdateView(mod);
        }

        foreach (var res in item.sellPrice.resources)
            Instantiate(resourceAmountWidget, sellPanel)
                .GetComponent<ResourceAmountWidget>()
                .UpdateView(res);

        foreach (var res in item.deconstructionPrice.resources)
            Instantiate(resourceAmountWidget, deconstructPanel)
                .GetComponent<ResourceAmountWidget>()
                .UpdateView(res);
    }

    private void DestroyAllChildren(Transform transform)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    private void Update()
    {
        if (trackPosition)
        {
            var pos = Input.mousePosition * canvas.scaleFactor;
            float xPivot = pos.x / Screen.width;
            float yPivot = pos.y / Screen.height;

            xPivot = xPivot > 0.5 ? 1 : 0;
            yPivot = yPivot > 0.5 ? 1 : 0;

            rectTransform.position = pos;
            rectTransform.pivot = new Vector2(xPivot, yPivot);
        }
    }
}
