using System;
using System.Collections;
using UnityEngine;
using TowerDefense.Combat;
using TowerDefense.Core;
using UnityEngine.EventSystems;

namespace TowerDefense.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    public class Enemy : MonoBehaviour, IDamageable, IPointerClickHandler
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
            Data = data;
            _target = target;

            _currentHealth = data.maxHealth;
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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

        public void OnPointerClick(PointerEventData eventData)
        {
            TakeDamage(25);
        }

        // ── VFX ─────────────────────────────────────────────────────────────

        private void PlayHitVFX()
        {
            if (_spriteRenderer != null)
                StartCoroutine(HitFlash());
        }

        private IEnumerator HitFlash()
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = _originalColor;
        }

        // Add to top of Die()
        private void Die()
        {
            IsDead = true;

            if (Data.deathVFXPrefab != null)
            {
                var vfx = PoolManager.Instance.Get("DeathVFX");
                if (vfx != null)
                    vfx.transform.position = transform.position;
                // VFXAutoReturn component handles returning to pool
            }

            GameManager.Instance?.RegisterKill(Data.scoreValue);
            OnDeath?.Invoke(this);
            GetComponent<ObjectToPool>()?.ReturnToPool();
        }

        // Called by EnemySpawner after Get() to reset state
        public void ResetState(EnemyData data, Transform target)
        {
            IsDead = false;
            Data = data;
            _target = target;
            _currentHealth = data.maxHealth;


            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                _originalColor = _spriteRenderer.color;
            }

            _spriteRenderer.color = _originalColor;
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