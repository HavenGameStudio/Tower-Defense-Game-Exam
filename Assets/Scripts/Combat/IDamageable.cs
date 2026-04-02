namespace TowerDefense.Combat
{
    /// <summary>
    /// Anything that can receive damage implements this.
    /// Towers never reference Enemy or PlayerBase directly — only IDamageable.
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(float amount);
        bool IsDead { get; }
    }
}