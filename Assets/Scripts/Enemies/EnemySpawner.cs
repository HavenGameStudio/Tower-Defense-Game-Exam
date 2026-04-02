using TowerDefense.Core;
using UnityEngine;

namespace TowerDefense.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private EnemyData[] enemyTypes;
        [SerializeField] private float spawnInterval = 2f;

        [Header("References")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform target; // PlayerBase transform

        private float _timer;
        private bool _stopped;

        public void StopSpawning() => _stopped = true;

        private void Update()
        {
            if (_stopped) return;

            _timer += Time.deltaTime;
            if (_timer >= spawnInterval)
            {
                _timer = 0f;
                SpawnFromData(enemyTypes[Random.Range(0, enemyTypes.Length)]);
            }
        }

        private void SpawnEnemy()
        {
            if (enemyTypes == null || enemyTypes.Length == 0) return;

            var data  = enemyTypes[Random.Range(0, enemyTypes.Length)];
            var go    = Instantiate(data.enemyPrefab, spawnPoint.position, Quaternion.identity);
            var enemy = go.GetComponent<Enemy>();

            if (enemy == null)
            {
                Debug.LogError($"Enemy prefab '{data.enemyPrefab.name}' is missing an Enemy component.");
                return;
            }

            enemy.Initialize(data, target);
        }
        
        private void SpawnFromData(EnemyData data)
        {
            var pooled = PoolManager.Instance.Get(data.poolKey);
            if (pooled == null) return;

            var enemy = pooled.GetComponent<Enemy>();
            if (enemy == null) return;

            pooled.transform.position = spawnPoint.position;
            enemy.ResetState(data, target);
        }
    }
}