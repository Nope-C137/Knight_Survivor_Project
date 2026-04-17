using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] private int currentScore = 0;
    [SerializeField] private int currentKills = 0;

    [Header("Events (dùng cho UI)")]
    public UnityEvent<int> OnScoreChanged;
    public UnityEvent<int> OnKillChanged;

    // Properties để đọc từ bên ngoài
    public int CurrentScore => currentScore;
    public int CurrentKills => currentKills;

    // Singleton đơn giản (dễ truy cập từ mọi nơi)
    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);   // Giữ lại khi chuyển scene (nếu cần)
    }

    /// <summary>
    /// Thêm điểm
    /// </summary>
    public void AddScore(int amount)
    {
        if (amount <= 0) return;

        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);

        // Debug
        Debug.Log($"Score +{amount} → Total: {currentScore}");
    }

    /// <summary>
    /// Thêm kill (thường đi kèm + điểm)
    /// </summary>
    public void AddKill(int scoreBonus = 100)
    {
        currentKills++;
        AddScore(scoreBonus);                    // Tự động cộng điểm khi kill

        OnKillChanged?.Invoke(currentKills);

        Debug.Log($"Kill +1 → Total Kills: {currentKills}");
    }

    /// <summary>
    /// Reset tất cả điểm và kill (dùng khi restart game)
    /// </summary>
    public void ResetStats()
    {
        currentScore = 0;
        currentKills = 0;
        OnScoreChanged?.Invoke(0);
        OnKillChanged?.Invoke(0);
    }

    /// <summary>
    /// Lưu điểm cao nhất (High Score)
    /// </summary>
    public void SaveHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
            Debug.Log($"New High Score: {currentScore}");
        }
    }

    public int GetHighScore() => PlayerPrefs.GetInt("HighScore", 0);
}