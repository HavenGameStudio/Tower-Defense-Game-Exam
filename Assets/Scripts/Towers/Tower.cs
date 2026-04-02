using System;
using TowerDefense.Combat;
using UnityEngine;

namespace TowerDefense.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        [Header("Config")] 
        [SerializeField] protected TowerData data;

        private float _fireCooldown;

        // ── Abstract ────────────────────────────────────────────────────────

        /// <summary>
        /// Called when the tower is ready to fire.
        /// Subclasses define what "firing" means for their type.
        /// </summary>
        /// <param name="target"></param>
        protected abstract void Fire(IDamageable target);

        // ── Update Loop ────────────────────────────────────────────────

        private void Update()
        {
            _fireCooldown -= Time.deltaTime;

            if (_fireCooldown <= 0f && TryGetTarget(out var target))
            {
                _fireCooldown = 1f / data.fireRate;
                Fire(target);
            }
        }

        // ── Target Detection ────────────────────────────────────────────────

        private bool TryGetTarget(out IDamageable target)
        {
            target = null;

            var hits = Physics2D.OverlapCircleAll(transform.position, data.range);

            foreach (var hit in hits)
            {
                var damageable = hit.GetComponent<IDamageable>();
                if (damageable != null && !damageable.IsDead)
                {
                    target = damageable;
                    return true;
                }
            }

            return false;
        }

        // ── Gizmos ──────────────────────────────────────────────────────────

        /// <summary>
        /// Draws gizmos on Edit mode.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (data == null) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, data.range);
        }
    }
}