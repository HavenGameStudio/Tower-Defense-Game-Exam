using TMPro;
using UnityEngine;
using TowerDefense.Core;

namespace TowerDefense.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Score")] [SerializeField] private TMP_Text scoreText;

        [Header("Game Over")] [SerializeField] private GameObject gameOverPanel;

        // ── Lifecycle ────────────────────────────────────────────────────────

        private void Start()
        {
            GameManager.Instance.OnScoreChanged += UpdateScore;
            GameManager.Instance.OnGameOver += ShowGameOver;

            // Init state
            gameOverPanel.SetActive(false);
            UpdateScore(0);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnScoreChanged -= UpdateScore;
            GameManager.Instance.OnGameOver -= ShowGameOver;
        }

        // ── Handlers ─────────────────────────────────────────────────────────

        private void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
        }
    }
}