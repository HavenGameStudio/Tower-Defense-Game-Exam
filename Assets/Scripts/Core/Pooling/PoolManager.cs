using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Core
{
    /// <summary>
    /// Owns one ObjectPooler per prefab type.
    /// EnemySpawner and Towers request objects by prefab reference.
    /// </summary>
    public class PoolManager : Singleton<PoolManager>
    {
        [System.Serializable]
        private struct PoolEntry
        {
            public string     key;
            public ObjectPooler pooler;
        }

        [SerializeField] private List<PoolEntry> pools;

        private Dictionary<string, ObjectPooler> _poolMap;

        protected override void Awake()
        {
            base.Awake();
            _poolMap = new Dictionary<string, ObjectPooler>();

            foreach (var entry in pools)
            {
                entry.pooler.Initialize();
                _poolMap[entry.key] = entry.pooler;
            }
        }

        // ── Public API ───────────────────────────────────────────────────────

        public ObjectToPool Get(string key)
        {
            if (_poolMap.TryGetValue(key, out var pooler))
                return pooler.GetObjectFromPool();

            Debug.LogError($"PoolManager: No pool found for key '{key}'");
            return null;
        }

        public void Return(string key, ObjectToPool obj)
        {
            if (_poolMap.TryGetValue(key, out var pooler))
                pooler.ReturnObjectToPool(obj);
            else
                obj.gameObject.SetActive(false);
        }
    }
}