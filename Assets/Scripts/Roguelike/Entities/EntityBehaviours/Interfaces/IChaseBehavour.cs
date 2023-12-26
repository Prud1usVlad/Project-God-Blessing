using UnityEngine;

public interface IChaseBehaviour
{
    public void ChasePlayer();

    public void Initialize(Transform player);

    public bool CheckPlayerSightable();

    public void OnDeathEvent();

#if UNITY_EDITOR
    public float GetSightRange();
#endif
}