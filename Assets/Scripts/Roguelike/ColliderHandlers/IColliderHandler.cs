using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Roguelike.Entities.Player;
using UnityEngine;

public interface IColliderHandler
{
    public Transform InteractTransform { get;}
    public void Interact();
}
