using UnityEngine;

namespace TowerDefense.Combat
{
    public class Projectile : MonoBehaviour
    {
        private IDamageable _target;
        private float _damage;
        private float _speed;
        private bool _isAOE;
        private float _aoeRadius;
        private LayerMask _enemyLayer;

        // ── Init ────────────────────────────────────────────────────────────

        public void Initialize(IDamageable target, float damage, float speed,
                               bool isAOE = false, float aoeRadius = 0f,
                               LayerMask enemyLayer = default)
        {
            _target     = target;
            _damage     = damage;
            _speed      = speed;
            _isAOE      = isAOE;
            _aoeRadius  = aoeRadius;
            _enemyLayer = enemyLayer;
        }

        // ── Movement ────────────────────────────────────────────────────────

        private void Update()
        {
            // Target died before we arrived — destroy self cleanly
            if (_target == null || _target.IsDead)
            {
                Destroy(gameObject);
                return;
            }

            var targetMono = _target as MonoBehaviour;
            if (targetMono == null) return;

            MoveToward(targetMono.transform.position);
            RotateToward(targetMono.transform.position);

            if (HasReachedTarget(targetMono.transform.position))
                OnImpact(targetMono.transform.position);
        }

        private void MoveToward(Vector3 targetPosition)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                _speed * Time.deltaTime
            );
        }

        private void RotateToward(Vector3 targetPosition)
        {
            var dir   = (targetPosition - transform.position).normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            // -90 offset assumes your arrow sprite points UP by default
            // change to angle if your sprite points RIGHT
        }

        private bool HasReachedTarget(Vector3 targetPosition)
        {
            return Vector2.Distance(transform.position, targetPosition) < 0.15f;
        }

        // ── Impact ──────────────────────────────────────────────────────────

        private void OnImpact(Vector3 impactPosition)
        {
            if (_isAOE)
                DealAOEDamage(impactPosition);
            else
                _target.TakeDamage(_damage);

            Destroy(gameObject);
        }

        private void DealAOEDamage(Vector3 impactPosition)
        {
            // Primary target takes full damage
            _target.TakeDamage(_damage);

            // Splash — half damage to nearby enemies
            var hits = Physics2D.OverlapCircleAll(impactPosition, _aoeRadius, _enemyLayer);
            foreach (var hit in hits)
            {
                var damageable = hit.GetComponent<IDamageable>();
                if (damageable != null && !damageable.IsDead && damageable != _target)
                    damageable.TakeDamage(_damage * 0.5f);
            }
        }
    }
}