using UnityEngine;
namespace TowerDefense.Towers
{
    [CreateAssetMenu(fileName = "New Tower Data", menuName = "TowerDefense/Tower Data")]
    public class TowerData: ScriptableObject
    {
        [Header("Base Stats")]
        public float damage = 20f;
        public float fireRate = 1f;
        public float range = 5f;
        
        [Header("AOE Towers")]
        public float aoeRadius = 2f;

        [Header("Multi-Target Towers")]
        public int maxTargets = 3;
    }
}