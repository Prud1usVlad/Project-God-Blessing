using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Player
{
    public class Weapon : MonoBehaviour
    {
        public float Damage;

        public GameObject Collider;

        private List<GameObject> _enemiesWithDamage = new List<GameObject>();

        public void StartAttack()
        {
            Collider.SetActive(true);
        }

        public void EndAttack()
        {
            Collider.SetActive(false);
            _enemiesWithDamage.Clear();
        }

        public bool TryHit(GameObject enemy)
        {
           if(_enemiesWithDamage.Contains(enemy))
           {
                return false;
           }

           _enemiesWithDamage.Add(enemy);

           return true;
        }
    }
}