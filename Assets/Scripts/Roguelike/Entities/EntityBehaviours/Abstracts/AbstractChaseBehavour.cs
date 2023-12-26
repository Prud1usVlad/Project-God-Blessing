using UnityEngine;

public abstract class AbstractChaseBehaviour : MonoBehaviour, IChaseBehaviour
{
    public float SightRange;
    public virtual void ChasePlayer()
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CheckPlayerSightable()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Initialize(Transform player)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnDeathEvent()
    {
        throw new System.NotImplementedException();
    }

#if UNITY_EDITOR
    public virtual float GetSightRange()
    {
        throw new System.NotImplementedException();
    }
#endif
}