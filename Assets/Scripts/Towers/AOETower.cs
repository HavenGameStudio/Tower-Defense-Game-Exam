using TowerDefense.Combat;
using UnityEngine;

namespace TowerDefense.Towers
{
    public class AOETower : Tower
    {
        [Header("VFX")] [SerializeField] private GameObject projectilePrefab;

        protected override void Fire(IDamageable primaryTarget)
        {
            var primaryMono = primaryTarget as MonoBehaviour;
            if (primaryMono == null) return;

            //Damage primary target
            primaryTarget.TakeDamage(data.damage);

            //Splash: hit everyting in aoeRadius around primary target
            var hits = Physics2D.OverlapCircleAll(primaryMono.transform.position, data.aoeRadius);

            foreach (var hit in hits)
            {
                var damageable = hit.GetComponent<IDamageable>();
                if (damageable != null && !damageable.IsDead && damageable != primaryTarget)
                {
                    damageable.TakeDamage(data.damage * 0.5f);
                }
            }

            //VFX
            if (projectilePrefab != null)
                Destroy(Instantiate(projectilePrefab, primaryMono.transform.position, Quaternion.identity), 1f);
        }

        private void OnDrawGizmosSelected()
        {
            if (data == null) return;

            //AOE splash radius preview around tower center (approximate)
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
            Gizmos.DrawWireSphere(transform.position, data.aoeRadius);
        }
    }
}