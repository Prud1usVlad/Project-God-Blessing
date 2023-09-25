using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    private bool isInBuildMode = false;
    
    public Building building;
    public GameObject dialogueBox;
    public bool isBuilt = false;

    public void Interact()
    {
        var canvas = GameObject.FindGameObjectWithTag("MainUi");
        var dial = Instantiate(dialogueBox, canvas.transform);
        var comp = dial.GetComponent<DialogueBox>();

        building.InitDialogue(comp);
    }

    public void OnBuildModeEnter()
    {
        Debug.Log("Enter build mode");
        isInBuildMode = true;
    }

    public void OnBuildModeExit()
    {
        isInBuildMode = false;
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) 
            && !isInBuildMode && isBuilt) 
        {
            Interact();
        }
    }
}
