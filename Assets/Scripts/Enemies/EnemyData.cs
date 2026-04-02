using UnityEngine;

namespace TowerDefense.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "TowerDefense/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Stats")]
        public float maxHealth = 100f;
        public float damage    = 10f;
        public float moveSpeed = 3f;

        [Header("Scoring")]
        public int scoreValue = 10;

        [Header("Visuals")]
        public GameObject deathVFXPrefab;
        
        [Header("Prefab")]
        public GameObject enemyPrefab;
        
        [Header("Pooling")]
        public string poolKey; // e.g. "Enemy_Basic", "Enemy_Tank"
    }
}