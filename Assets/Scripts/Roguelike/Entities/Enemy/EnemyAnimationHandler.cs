using System;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    public EnemyController EnemyController;

    public void OnPunchAnimationEnd()
    {
        EnemyController.OnPunchEnd?.Invoke();
    }

    public void OnThrowAnimationEnd()
    {
        EnemyController.OnThrowEnd?.Invoke();
    }

    public void OnThowEvent()
    {
        EnemyController.OnThrowEvent?.Invoke();
    }

    public void OnSpawnAnimationEnd()
    {
        EnemyController.OnSpawnEnd?.Invoke();
    }

    public void OnSpawnEvent()
    {
        EnemyController.OnSpawnEvent?.Invoke();
    }

    public void OnBiteAnimationEnd()
    {
        EnemyController.OnBiteEnd?.Invoke();
    }

    public void OnBiteEvent()
    {
        EnemyController.OnBiteEvent?.Invoke();
    }
}
