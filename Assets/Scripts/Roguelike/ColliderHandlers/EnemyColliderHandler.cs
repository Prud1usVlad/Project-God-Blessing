using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Roguelike.Entities.Player;
using UnityEngine;

public class EnemyColliderHandler : MonoBehaviour
{
    public EnemyController EnemyController;

    private List<GadgetController> _relatedGadgets = new List<GadgetController>();

    void Start()
    {
        EnemyController.OnDeathEvent += clear;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(TagHelper.ColliderTags.GadgetDamageAreaTag))
        {
            GadgetController gadgetController = other.transform.parent.parent.GetComponent<GadgetController>();
            EnemyController.TakeDamage(gadgetController.GadgetEnterDamage);
            gadgetController.TrappedEnemies.Add(EnemyController);
            _relatedGadgets.Add(gadgetController);
        }

        if (other.transform.tag.Equals(TagHelper.ColliderTags.SwordDamageAreaTag))
        {
            Weapon weapon = other.transform.parent.GetComponent<Weapon>();
            if (weapon.TryHit(gameObject))
            {
                EnemyController.TakeDamage(weapon.Damage);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag.Equals(TagHelper.ColliderTags.GadgetDamageAreaTag))
        {
            GadgetController gadgetController = other.transform.parent.parent.GetComponent<GadgetController>();
            gadgetController.TrappedEnemies.Remove(EnemyController);
            _relatedGadgets.Remove(gadgetController);
        }
    }

    private void clear()
    {
        foreach (GadgetController gadgetController in _relatedGadgets)
        {
            gadgetController.TrappedEnemies.Remove(EnemyController);
        }

        _relatedGadgets.Clear();
    }
}

