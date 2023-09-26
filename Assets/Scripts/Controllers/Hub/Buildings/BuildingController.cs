using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetInstanceID() == transform.GetInstanceID())
                {
                    Interact();
                }
            }
        }
    }
}
