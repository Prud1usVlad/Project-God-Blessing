using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Roguelike.Entities.Enemy;
using Assets.Scripts.Roguelike.Entities.Player;
using UnityEngine;

public class PlayerColliderHandler : MonoBehaviour
{
    public Player Player;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(TagHelper.ColliderTags.EnemyDamageAreaTag))
        {
            if (!PlayerStateHelper.Instance.IsInvincibleByDodge)
            {
                Player.Health -= other.transform.parent.GetComponent<EnemyDecorator>()
                    .EnemyParameters.EnemyBaseDamage;
            }
        }
    }
}

