using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Combat;

namespace TowerDefense.Towers
{
    public class MultiTargetTower : Tower
    {
        [Header("Projectile")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 10f;

        protected override void Fire(IDamageable primaryTarget)
        {
            if (projectilePrefab == null) return;

            var targets = GetTargetsInRange();

            foreach (var target in targets)
                SpawnProjectile(target);
        }

        private void SpawnProjectile(IDamageable target)
        {
            var go         = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var projectile = go.GetComponent<Projectile>();

            if (projectile == null)
            {
                Debug.LogError("Projectile prefab is missing a Projectile component.");
                Destroy(go);
                return;
            }

            projectile.Initialize(
                target: target,
                damage: data.damage,
                speed:  projectileSpeed
            );
        }

        private List<IDamageable> GetTargetsInRange()
        {
            var targets = new List<IDamageable>();
            var hits    = Physics2D.OverlapCircleAll(transform.position, data.range, enemyLayer);

            foreach (var hit in hits)
            {
                if (targets.Count >= data.maxTargets) break;

                var damageable = hit.GetComponent<IDamageable>();
                if (damageable != null && !damageable.IsDead)
                    targets.Add(damageable);
            }

            return targets;
        }
    }
}