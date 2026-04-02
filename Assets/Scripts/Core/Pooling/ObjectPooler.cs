using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Core
{
    public class ObjectPooler : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private ObjectToPool prefab;
        [SerializeField] private int poolCount       = 10;
        [SerializeField] private bool poolCanExpand  = true;
        [SerializeField] private bool nestUnderThis  = true;

        public event Action OnPoolInitialized;

        private Queue<ObjectToPool> _pool = new();

        // ── Lifecycle ────────────────────────────────────────────────────────

        public void Initialize()
        {
            var parent = nestUnderThis ? transform : null;
            for (int i = 0; i < poolCount; i++)
                SpawnSingleObject(parent);

            OnPoolInitialized?.Invoke();
        }

        // ── Internal ─────────────────────────────────────────────────────────

        private ObjectToPool SpawnSingleObject(Transform parent = null)
        {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
            obj.Initialization(this);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
            return obj;
        }

        // ── Public API ───────────────────────────────────────────────────────

        public ObjectToPool GetObjectFromPool()
        {
            if (_pool.Count > 0)
            {
                var obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            if (!poolCanExpand) return null;

            // Pool empty — expand
            var parent    = nestUnderThis ? transform : null;
            var newObj    = SpawnSingleObject(parent);
            _pool.Dequeue(); // remove the one we just enqueued
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        public void ReturnObjectToPool(ObjectToPool obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}