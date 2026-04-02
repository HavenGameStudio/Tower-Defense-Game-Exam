using UnityEngine;
using TowerDefense.Combat;
using TowerDefense.Core;

namespace TowerDefense.Towers
{
    public class AOETower : Tower
    {
        [Header("Projectile")]
        [SerializeField] private string projectilePoolKey = "Projectile";
        [SerializeField] private float projectileSpeed = 8f;

        protected override void Fire(IDamageable primaryTarget)
        {
            var pooled = PoolManager.Instance.Get(projectilePoolKey);
            if (pooled == null) return;

            pooled.transform.position = transform.position;
            pooled.transform.rotation = Quaternion.identity;

            var projectile = pooled.GetComponent<Projectile>();
            if (projectile == null) return;

            projectile.ResetState(
                target:     primaryTarget,
                damage:     data.damage,
                speed:      projectileSpeed,
                isAOE:      true,
                aoeRadius:  data.aoeRadius,
                enemyLayer: enemyLayer
            );
        }

        private void OnDrawGizmosSelected()
        {
            if (data == null) return;
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
            Gizmos.DrawWireSphere(transform.position, data.aoeRadius);
        }
    }
}