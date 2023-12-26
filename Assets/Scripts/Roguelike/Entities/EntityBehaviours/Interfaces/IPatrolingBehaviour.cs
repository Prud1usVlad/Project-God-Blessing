using UnityEngine;

public interface IPatrolingBehaviour
{
    public void Patroling();

    public void Initialize(Transform player);

    public void OnDeathEvent();
}