using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Needed for TextMeshPro
using UnityEngine.SceneManagement; // Needed if you want to restart later

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState { MainMenu, Playing, Paused, GameOver }

    public GameState CurrentState { get; private set; }

    public bool IsPlaying => CurrentState == GameState.Playing;

    public static event Action<GameState> OnGameStateChanged;


    public static int scoreKeeper = 0;
    public static int currentLives = 3;
    public static int currentRound = 1;

    [Header("Boss Settings")]
    public bool isBossLevel = false; // Check this ONLY in the Level_3 Inspector!
    public GameObject bossPrefab;
    private bool bossActive = false;

    [Header("Round Timer")]
    public float[] roundDurations = new float[] { 30f, 45f, 60f };
    public TextMeshProUGUI timeText;
    public float RoundTimeRemaining { get; private set; }
    private Coroutine roundTimerRoutine;

    [Header("Difficulty")]
    public float difficultyPerRound = 0.25f;
    public float difficultyPerLevel = 0.5f;
    public float DifficultyMultiplier { get; private set; } = 1f;

    [Header("UI Settings")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI missileText;
    public TextMeshProUGUI centerText;
    public TextMeshProUGUI livesText; // NEW: Make sure to create this text in Unity!
    public TextMeshProUGUI highScoreText;
    public GameObject restartButton;
    public GameObject mainmenuButton;

    [Header("Game Settings")]
    public int missilesToWin = 10;
    public int scorePerMissile = 100;
    public int maxRounds = 3; // NEW: Rounds per level
    public string nextLevelName; // NEW: Type "Level_2" (or your scene name) in the Inspector
    public int currentLevelNumber = 1;

    private int currentScore = 0;
    private int highScore;
    public bool isGameActive = true;
    public bool spawningEnabled = true;

    void Awake()
    {
        // This ensures there is only ever one Game Manager
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);

            // Listen for scene changes so we can set the correct state automatically
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentScore = scoreKeeper; // restores score

        if (roundDurations == null || roundDurations.Length == 0)
        {
            roundDurations = new float[] { 30f, 45f, 60f };
        }

        maxRounds = Mathf.Clamp(maxRounds, 1, roundDurations.Length);

        // loads saved highscore
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        ApplyLevelContentConfig();
        StartNewRound();

        UpdateUI();

        // Start adding survival points every 1 second
        InvokeRepeating("AddSurvivalPoints", 1f, 1f);
    }

    void StartNewRound()
    {
        isGameActive = true;
        spawningEnabled = true;
        bossActive = false;

        ApplyDifficulty();
        RoundTimeRemaining = GetRoundDurationSeconds();

        if (roundTimerRoutine != null) StopCoroutine(roundTimerRoutine);
        roundTimerRoutine = StartCoroutine(RoundTimerLoop());
    }

    IEnumerator RoundTimerLoop()
    {
        while (true)
        {
            if (!isGameActive) yield break;

            if (IsPlaying && !bossActive)
            {
                RoundTimeRemaining -= Time.deltaTime;
                if (RoundTimeRemaining <= 0f)
                {
                    RoundTimeRemaining = 0f;
                    UpdateUI();
                    OnRoundTimerExpired();
                    yield break;
                }

                UpdateUI();
            }

            yield return null;
        }
    }

    float GetRoundDurationSeconds()
    {
        int index = Mathf.Clamp(currentRound - 1, 0, roundDurations.Length - 1);
        return Mathf.Max(1f, roundDurations[index]);
    }

    void OnRoundTimerExpired()
    {
        if (isBossLevel && currentRound == maxRounds)
        {
            SpawnBoss();
            return;
        }

        RoundComplete();
    }

    void ApplyDifficulty()
    {
        int levelIndex = Mathf.Max(0, currentLevelNumber - 1);
        int roundIndex = Mathf.Max(0, currentRound - 1);
        DifficultyMultiplier = 1f + (levelIndex * difficultyPerLevel) + (roundIndex * difficultyPerRound);
    }

    void ApplyLevelContentConfig()
    {
        LevelContentConfig config = FindFirstObjectByType<LevelContentConfig>();
        if (config != null) config.Apply();
    }

    void AddSurvivalPoints()
    {
        //if (isGameActive)
        if (isGameActive && CurrentState == GameState.Playing)
        {
            currentScore += 1; // +1 point for every second you survive
            UpdateUI();
        }
    }

    bool CheckHighScore() {
        if (currentScore > highScore) {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            centerText.text = "NEW HIGHSCORE!";
            centerText.color = Color.yellow;

            return true;
        }
        return false;
    }

    public void AddScore(int amount)
    {
        if (!isGameActive) return;

        currentScore += amount;
        UpdateUI();
    }

    public void MissileDestroyed()
    {
        if (!isGameActive) return;

        // 1. Add Score
        AddScore(scorePerMissile); // Or 100, whatever you prefer

        UpdateUI();
    }

    void SpawnBoss()
    {
        bossActive = true;
        spawningEnabled = false;
        centerText.text = "WARNING: ENEMY FLAGSHIP APPROACHING";
        centerText.color = Color.red;

        Invoke(nameof(ClearCenterText), 3f);
        
        // Spawn the Boss just off the right edge of the screen
        Instantiate(bossPrefab, new Vector3(12f, 0f, 0f), Quaternion.identity);
    }

    void ClearCenterText()
    {
        // We only clear it if the player is still alive 
        // (so we don't accidentally erase a "GAME OVER" screen)
        if (isGameActive)
        {
            centerText.text = "";
        }
    }

    public void BossDefeated()
    {
        AddScore(1000); // Big point bonus
        bossActive = false;
        RoundComplete(); // This will trigger the "Level Complete" screen
    }

    public void PlayerDied()
    {
        isGameActive = false;
        spawningEnabled = false;
        currentLives--; // Lose of life
        SetState(GameState.GameOver); // player death
        //centerText.text = "GAME OVER";
        //centerText.color = Color.red;
        CancelInvoke("AddSurvivalPoints"); // Stop giving points

        restartButton.SetActive(true);
        mainmenuButton.SetActive(true);

        //scoreKeeper = 0;

        if (currentLives > 0)
        {
            scoreKeeper = currentScore; // saves score in between lives

            centerText.text = "SHIP DESTROYED\nLIVES LEFT: " + currentLives;
            centerText.color = Color.yellow;
            restartButton.GetComponentInChildren<TextMeshProUGUI>().text = "DEPLOY BACKUP";
        }

        else
        {
            bool isNewHigh = CheckHighScore(); // check score before games ends

            if (!isNewHigh) {
                centerText.text = "GAME OVER";
                centerText.color = Color.red;
            }

            restartButton.GetComponentInChildren<TextMeshProUGUI>().text = "RESTART CAMPAIGN";
            
            // Wipe memory for a totally fresh run
            scoreKeeper = 0;
            currentLives = 3;
            currentRound = 1;
        }
    }

    //void WinGame()
    //{
    //    isGameActive = false;
    //    centerText.text = "YOU WIN PILOT";
    //    centerText.color = Color.green;
    //    restartButton.SetActive(true);
    //    mainmenuButton.SetActive(true);
    //    CancelInvoke("AddSurvivalPoints");
    //    scoreKeeper = currentScore;
    //}
    void RoundComplete()
    {
        isGameActive = false;
        spawningEnabled = false;
        scoreKeeper = currentScore; // Save score
        SetState(GameState.GameOver); // Use GameOver state to pause time and show menus
        CancelInvoke("AddSurvivalPoints");

        restartButton.SetActive(true);
        mainmenuButton.SetActive(true);

        if (currentRound < maxRounds)
        {
            centerText.text = "ROUND " + currentRound + " CLEARED";
            centerText.color = Color.green;
            restartButton.GetComponentInChildren<TextMeshProUGUI>().text = "NEXT ROUND";
            currentRound++; 
        }
        else
        {
            bool isNewHigh = CheckHighScore(); // check score after level complete 

            if (!isNewHigh) {
                centerText.text = "LEVEL COMPLETE!";
                centerText.color = Color.cyan;
            }

            restartButton.GetComponentInChildren<TextMeshProUGUI>().text = "NEXT SECTOR";
            currentRound = 1; // Reset rounds for the new level
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time unpauses
        string buttonAction = restartButton.GetComponentInChildren<TextMeshProUGUI>().text;

        if (buttonAction == "NEXT SECTOR")
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        // This reloads the current scene (resets everything)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;

        int seconds = Mathf.CeilToInt(RoundTimeRemaining);
        if (timeText != null) timeText.text = "Time: " + seconds;
        if (missileText != null) missileText.text = "Time: " + seconds;

        if (livesText != null) {
            livesText.text = "Lives: " + currentLives;
        }

        if (highScoreText != null) {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    void OnDestroy()
    {
        // Clean up event subscription when object is destroyed
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Runs every time a new scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu") 
        {
            SetState(GameState.MainMenu);
        }
        //else if (scene.name == "Game") 
        else if (scene.name == "Game" || scene.name.Contains("Level"))
        {
            SetState(GameState.Playing);
            ApplyLevelContentConfig();
            StartNewRound();
        }
    }
    void Update()
    {
        // Only allow pausing while in the Game scene / Playing or Paused state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing) PauseGame();
            else if (CurrentState == GameState.Paused) ResumeGame();
        }
    }

public void SetState(GameState newState)
    {
        CurrentState = newState;

        // Control time based on state
        Time.timeScale = (newState == GameState.Paused || newState == GameState.GameOver) ? 0f : 1f;

        Debug.Log("State changed to: " + newState);

        OnGameStateChanged?.Invoke(newState);
    }

    public void PauseGame() => SetState(GameState.Paused);
    public void ResumeGame() => SetState(GameState.Playing);

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
}
