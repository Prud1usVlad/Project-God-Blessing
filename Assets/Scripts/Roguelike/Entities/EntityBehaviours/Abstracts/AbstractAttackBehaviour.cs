using System;
using UnityEngine;

[Serializable]
public abstract class AbstractAttackBehaviour : MonoBehaviour, IAttackBehaviour
{
    public GameObject AttackCollider;
    public virtual AttackParameters AttackParameters { get; set; }

    public int Priority;
    public bool IsAttackCooldown;
    public AttackType AttackType;

    public virtual void Attack()
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CheckAttackPossibility()
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CheckAttackRange()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Initialize(Transform player)
    {
        throw new System.NotImplementedException();
    }

#if UNITY_EDITOR
    public virtual float GetAttackRange()
    {
        throw new System.NotImplementedException();
    }
#endif
}