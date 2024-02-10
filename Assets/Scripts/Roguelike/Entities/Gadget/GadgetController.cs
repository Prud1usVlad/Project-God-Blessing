using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class GadgetController : MonoBehaviour
{
    public float ThrowSpeed = 1f;
    public float ThrowPositionPrecision = 0.1f;
    public float TimeToGadgetWork = 10f;
    public float ActionAreaSize = 4;
    public float GadgetEnterDamage = 10f;
    public float GadgetDamagePerTick = 1f;
    public float TickTime = 0.05f;
    private float currentTime = 0f;

    public List<EnemyController> TrappedEnemies = new List<EnemyController>();

    [Header("Game objects")]
    public GameObject GadgetModel;
    public GameObject GadgetVFXArea;
    public VisualEffect GadgetVFX;
    public MeshCollider GadgetActionArea;

    private Vector3? _destinationPoint = null;

    private void Start()
    {
        GadgetModel.SetActive(false);
        GadgetVFXArea.SetActive(false);
    }

    public void Throw(Vector3 destinationPoint)
    {
        _destinationPoint = destinationPoint;
        GadgetModel.SetActive(true);
    }

    private void Update()
    {
        if (_destinationPoint.HasValue)
        {
            Vector3 direction = _destinationPoint.Value - transform.position;

            Vector3 normalizedDirection = direction.normalized;

            Vector3 newPosition = transform.position + normalizedDirection * ThrowSpeed * Time.deltaTime;

            //Debug.Log($"{direction}\n{normalizedDirection}\n{newPosition}\n");

            transform.position = newPosition;
            Vector3 deltaVector = transform.position - _destinationPoint.Value;
            if (Math.Abs(deltaVector.x) <= ThrowPositionPrecision
                && Math.Abs(deltaVector.y) <= ThrowPositionPrecision
                && Math.Abs(deltaVector.z) <= ThrowPositionPrecision)
            {
                transform.position = _destinationPoint.Value;
                transform.rotation = Quaternion.identity;
                _destinationPoint = null;

                GadgetModel.SetActive(false);
                GadgetActionArea.transform.localScale = new Vector3(ActionAreaSize, 0.4f, ActionAreaSize);
                GadgetVFX.SetVector3("AreaScales", GadgetActionArea.transform.localScale);
                GadgetVFXArea.SetActive(true);

                Destroy(gameObject, TimeToGadgetWork);
            }
        }


        if (currentTime >= TickTime)
        {
            int ticks = (int)(currentTime/TickTime);
            foreach (EnemyController enemyController in TrappedEnemies)
            {
                enemyController?.TakeDamage(GadgetDamagePerTick * ticks);
            }

            currentTime -= ticks;
        }
        else
        {
            currentTime += Time.deltaTime;
        }

    }
}