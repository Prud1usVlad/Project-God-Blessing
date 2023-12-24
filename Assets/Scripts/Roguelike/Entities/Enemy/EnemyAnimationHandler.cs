using System;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    public EnemyController EnemyController;

    public void OnPunchAnimationEnd()
    {
        EnemyController.OnAttackEnd?.Invoke();
    }
}
