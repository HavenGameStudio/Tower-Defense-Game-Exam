using System;
using UnityEngine;
using TowerDefense.Enemies;

namespace TowerDefense.Core
{
    public enum GameState { Playing, Lost }

    public class GameManager : Singleton<GameManager>
    {
        [Header("References")]
        [SerializeField] private PlayerBase playerBase;
        [SerializeField] private EnemySpawner enemySpawner;

        public event Action<int> OnScoreChanged;
        public event Action      OnGameOver;

        public GameState State { get; private set; } = GameState.Playing;
        public int Score       { get; private set; }

        // ── Lifecycle ────────────────────────────────────────────────────────

        protected override void Awake()
        {
            base.Awake(); // always call — this is where InitializeSingleton runs
        }

        private void Start()
        {
            playerBase.OnDestroyed += HandleBaseDestroyed;
        }

        private void OnDestroy()
        {
            if (playerBase != null)
                playerBase.OnDestroyed -= HandleBaseDestroyed;
        }

        // ── Public API ───────────────────────────────────────────────────────

        public void RegisterKill(int scoreValue)
        {
            if (State != GameState.Playing) return;

            Score += scoreValue;
            OnScoreChanged?.Invoke(Score);
        }

        // ── Handlers ─────────────────────────────────────────────────────────

        private void HandleBaseDestroyed()
        {
            if (State == GameState.Lost) return;

            State = GameState.Lost;
            
            Time.timeScale = 0f;
            
            enemySpawner.StopSpawning();
            OnGameOver?.Invoke();
        }
    }
}