using Assets.Scripts.ResourceSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RogueLike/ResourceData", fileName = "ResourceData")]
public class ResourceData : ScriptableObject
{
    public GameObject ResourceIcon;
    public ResourceName Name;
}
