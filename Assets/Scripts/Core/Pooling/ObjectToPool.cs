using UnityEngine;

namespace TowerDefense.Core
{
    /// <summary>
    /// Attached to any prefab that will be managed by an ObjectPooler.
    /// Holds a reference back to its pool for self-return.
    /// </summary>
    public class ObjectToPool : MonoBehaviour
    {
        public ObjectPooler OwnerPool { get; private set; }

        public void Initialization(ObjectPooler pool)
        {
            OwnerPool = pool;
        }

        /// <summary>
        /// Returns this object to its pool. Call instead of Destroy().
        /// </summary>
        public void ReturnToPool()
        {
            if (OwnerPool == null)
            {
                Destroy(gameObject);
                return;
            }
            OwnerPool.ReturnObjectToPool(this);
        }
    }
}