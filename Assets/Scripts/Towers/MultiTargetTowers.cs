using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Combat;
using TowerDefense.Core;

namespace TowerDefense.Towers
{
    public class MultiTargetTower : Tower
    {
        [Header("Projectile")]
        [SerializeField] private string projectilePoolKey = "Projectile";
        [SerializeField] private float projectileSpeed = 10f;

        protected override void Fire(IDamageable primaryTarget)
        {
            var targets = GetTargetsInRange();
            foreach (var target in targets)
                SpawnProjectile(target);
        }

        private void SpawnProjectile(IDamageable target)
        {
            var pooled = PoolManager.Instance.Get(projectilePoolKey);
            if (pooled == null) return;

            pooled.transform.position = transform.position;
            pooled.transform.rotation = Quaternion.identity;

            var projectile = pooled.GetComponent<Projectile>();
            if (projectile == null) return;

            projectile.ResetState(
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