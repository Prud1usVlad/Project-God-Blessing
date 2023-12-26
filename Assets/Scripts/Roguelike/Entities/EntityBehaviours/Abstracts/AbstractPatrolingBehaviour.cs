using UnityEngine;

public abstract class AbstractPatrolingBehaviour : MonoBehaviour, IPatrolingBehaviour
{
    public virtual void Initialize(Transform player)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Patroling()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnDeathEvent()
    {
        throw new System.NotImplementedException();
    }
}