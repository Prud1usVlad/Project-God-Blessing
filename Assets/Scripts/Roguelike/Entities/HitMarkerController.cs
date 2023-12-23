using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarkerController : MonoBehaviour
{
    public float ExistingTime = 3f;

    void Start()
    {
        Destroy(gameObject, ExistingTime);
    }
}
