using System;
using System.Collections;
using UnityEngine;
using TowerDefense.Combat;

namespace TowerDefense.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        public event Action<Enemy> OnDeath;

        public EnemyData Data { get; private set; }
        public bool IsDead { get; private set; }

        private float _currentHealth;
        private Transform _target;
        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;

        // ── Init ────────────────────────────────────────────────────────────

        public void Initialize(EnemyData data, Transform target)
        {
            Data    = data;
            _target = target;

            _currentHealth   = data.maxHealth;
            _spriteRenderer  = GetComponentInChildren<SpriteRenderer>();

            if (_spriteRenderer != null)
                _originalColor = _spriteRenderer.color;
        }

        // ── Movement ────────────────────────────────────────────────────────

        private void Update()
        {
            if (IsDead || _target == null) return;
            MoveTowardTarget();
        }

        private void MoveTowardTarget()
        {
            var direction = (_target.position - transform.position).normalized;
            transform.position += direction * (Data.moveSpeed * Time.deltaTime);
        }

        // ── IDamageable ─────────────────────────────────────────────────────

        public void TakeDamage(float amount)
        {
            if (IsDead) return;

            _currentHealth -= amount;
            PlayHitVFX();

            if (_currentHealth <= 0f)
                Die();
        }

        // ── Click to Damage ─────────────────────────────────────────────────

        private void OnMouseDown()
        {
            TakeDamage(25f);
        }

        // ── VFX ─────────────────────────────────────────────────────────────

        private void PlayHitVFX()
        {
            if (_spriteRenderer != null)
                StartCoroutine(HitFlash());

            if (Data.hitVFXPrefab != null)
                Destroy(Instantiate(Data.hitVFXPrefab, transform.position, Quaternion.identity), 2f);
        }

        private IEnumerator HitFlash()
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = _originalColor;
        }

        private void Die()
        {
            IsDead = true;

            if (Data.deathVFXPrefab != null)
                Destroy(Instantiate(Data.deathVFXPrefab, transform.position, Quaternion.identity), 2f);

            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }

        // ── Arrival ─────────────────────────────────────────────────────────

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            var target = other.GetComponent<IDamageable>();
            if (target == null || target.IsDead) return;

            target.TakeDamage(Data.damage);
            Die();
        }
    }
}