using System;
using Assets.Scripts.Helpers.Roguelike;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Player
{
    public class Player : MonoBehaviour
    {
        [Min(0f)]
        [SerializeField]
        private float _health = 100f;
        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
                HealthChangeEvent?.Invoke();
            }
        }

        public Action HealthChangeEvent;

        private float _baseHealthValue;

        private void Start()
        {
            _baseHealthValue = _health;

            RestartGameHelper.Instance.Restart += delegate ()
            {
                Health = _baseHealthValue;
            };
        }
    }
}