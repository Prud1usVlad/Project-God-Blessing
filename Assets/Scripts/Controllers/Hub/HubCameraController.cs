using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HubCameraController : MonoBehaviour
{
    private Vector3 newPos;
    private float cameraDistance;
    private Vector3 newCameraPos;

    public Camera childCamera;
    public float movementSpeed = 0.3f;
    public float movementTime = 5f;

    public float maxCameraDistance = 10;
    public float minCameraDistance = 2;

    public float zoomTime = 4;
    public float zoomSpeed = 1;
    public float zoomSpeedEffectMultiplier = 5;

    // Start is called before the first frame update
    void Start()
    {
        newPos = transform.position;
        cameraDistance = (maxCameraDistance + minCameraDistance) / 2;

        newCameraPos = childCamera.transform.localPosition.normalized * cameraDistance;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
        {
            newPos += CalcNewPos(transform.forward, movementSpeed);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPos += CalcNewPos(transform.forward, -movementSpeed);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPos += CalcNewPos(transform.right, -movementSpeed);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPos += CalcNewPos(transform.right, movementSpeed);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            cameraDistance += Input.mouseScrollDelta.y * zoomSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance,
                minCameraDistance, maxCameraDistance);

            newCameraPos = newCameraPos.normalized * cameraDistance;
        }

        transform.position = Vector3.Lerp(transform.position,
            newPos, Time.deltaTime * movementTime);

        childCamera.transform.localPosition = Vector3.Lerp(
            childCamera.transform.localPosition, newCameraPos, Time.deltaTime * zoomTime);
    }

    private Vector3 CalcNewPos(Vector3 direction, float speed)
    {
        // Multiplication by camera distance is for speed change while zooming
        return direction * speed * (cameraDistance / zoomSpeedEffectMultiplier);
    }
}
