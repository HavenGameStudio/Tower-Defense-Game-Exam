using System;
using UnityEngine;
using TowerDefense.Combat;

namespace TowerDefense.Core
{
    public class PlayerBase : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        [SerializeField] private float maxHealth = 100f;

        public event Action OnDestroyed;

        private float _currentHealth;
        public bool IsDead { get; private set; }

        private void Start()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;

            _currentHealth -= amount;

            if (_currentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            IsDead = true;
            OnDestroyed?.Invoke();
        }
    }
}