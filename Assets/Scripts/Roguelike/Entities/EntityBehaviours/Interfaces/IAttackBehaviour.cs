using UnityEngine;

public interface IAttackBehaviour
{
    public void Initialize(Transform player);
    public void Attack();
    public bool CheckAttackPossibility();
    public bool CheckAttackRange();
#if UNITY_EDITOR
    public float GetAttackRange();
#endif
}