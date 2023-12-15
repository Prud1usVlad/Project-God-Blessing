using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        isInBuildMode = true;
    }

    public void OnBuildModeExit()
    {
        isInBuildMode = false;
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0) && !isInBuildMode && 
        //    !EventSystem.current.IsPointerOverGameObject())
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.transform.GetInstanceID() == transform.GetInstanceID())
        //        {
        //            Interact();
        //        }
        //    }
        //}
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            Interact();
    }
}
