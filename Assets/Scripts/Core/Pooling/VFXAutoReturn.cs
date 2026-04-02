using UnityEngine;

namespace TowerDefense.Core
{
    /// <summary>
    /// Attach to any VFX prefab that lives in a pool.
    /// Automatically returns to pool when the particle system finishes.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class VFXAutoReturn : MonoBehaviour
    {
        private ParticleSystem _particles;
        private ObjectToPool _poolObject;

        private void Awake()
        {
            _particles  = GetComponent<ParticleSystem>();
            _poolObject = GetComponent<ObjectToPool>();
        }

        private void OnEnable()
        {
            // Play On Awake handles playback — just wait for it to finish
            _particles.Play();
        }

        private void Update()
        {
            if (!_particles.IsAlive())
                ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (_poolObject != null)
                _poolObject.ReturnToPool();
            else
                gameObject.SetActive(false);
        }
    }
}