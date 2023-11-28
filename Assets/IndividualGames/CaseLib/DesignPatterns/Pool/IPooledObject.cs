namespace IndividualGames.CaseLib.Behavior.GameSystem
{
    /// <summary>
    /// Object that can be pooled.
    /// </summary>
    public interface IPooledObject
    {
        /// <summary> Return to this pool when finished. </summary>
        public abstract IPool Pool { get; set; }
        /// <summary> Ideally called just after retrieved from pool. </summary>
        abstract void Retrieved(IPool returnPool);
        /// <summary> Ideally returned to pool inside. </summary>
        abstract void ReturnToPool();
    }
}