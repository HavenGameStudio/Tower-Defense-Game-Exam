using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Combat;

namespace TowerDefense.Towers
{
    public class MultiTargetTower : Tower
    {
        [Header("VFX")]
        [SerializeField] private GameObject projectilePrefab;

        protected override void Fire(IDamageable primaryTarget)
        {
            var targets = GetTargetsInRange();

            foreach (var target in targets)
            {
                target.TakeDamage(data.damage);

                // VFX per target
                if (projectilePrefab != null)
                {
                    var targetMono = target as MonoBehaviour;
                    if (targetMono != null)
                        Destroy(Instantiate(projectilePrefab, targetMono.transform.position, Quaternion.identity), 1f);
                }
            }
        }

        private List<IDamageable> GetTargetsInRange()
        {
            var targets = new List<IDamageable>();
            var hits    = Physics2D.OverlapCircleAll(transform.position, data.range);

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