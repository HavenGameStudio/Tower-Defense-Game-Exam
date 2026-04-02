using UnityEngine;
using TowerDefense.Combat;

namespace TowerDefense.Towers
{
    public class AOETower : Tower
    {
        [Header("Projectile")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 8f;

        protected override void Fire(IDamageable primaryTarget)
        {
            if (projectilePrefab == null) return;

            var go         = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var projectile = go.GetComponent<Projectile>();

            if (projectile == null)
            {
                Debug.LogError("Projectile prefab is missing a Projectile component.");
                Destroy(go);
                return;
            }

            projectile.Initialize(
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