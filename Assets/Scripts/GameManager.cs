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

    public static event Action<GameState> OnGameStateChanged;

    [Header("Player Settings")]
    public GameObject playerPrefab; // Drag your Jet Prefab here in the Inspector!
    public Vector3 playerSpawnPosition = new Vector3(-6f, 0f, 0f); // Where the jet starts

    public static int scoreKeeper = 0;
    public static int currentLives = 3;
    public static int currentRound = 1;

    [Header("Round Timer")]
    public float[] roundDurations = new float[] { 30f, 45f, 60f };
    public TextMeshProUGUI timeText;
    public float RoundTimeRemaining { get; private set; }
    private Coroutine roundTimerRoutine;

    [Header("Boss Settings")]
    public bool isBossLevel = false; // Check this ONLY in the Level_3 Inspector!
    public GameObject bossPrefab;
    private bool bossActive = false;

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
    private int currentMissilesLeft;
    private int highScore;
    public bool isGameActive = true;

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

        if (timeText == null)
        {
            GameObject foundTimeText = GameObject.Find("TimeText");
            if (foundTimeText != null) timeText = foundTimeText.GetComponent<TextMeshProUGUI>();
        }

        if (highScoreText == null)
        {
            GameObject foundHighScoreText = GameObject.Find("HighScoreText");
            if (foundHighScoreText != null) highScoreText = foundHighScoreText.GetComponent<TextMeshProUGUI>();
        }

        // loads saved highscore
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        StartNewRound();

        UpdateUI();

        // Start adding survival points every 1 second
        InvokeRepeating("AddSurvivalPoints", 1f, 1f);
    }

    void StartNewRound()
    {
        isGameActive = true;
        bossActive = false;

        currentMissilesLeft = missilesToWin + (currentRound * 2);
        RoundTimeRemaining = GetRoundDurationSeconds();

        if (roundTimerRoutine != null) StopCoroutine(roundTimerRoutine);
        roundTimerRoutine = StartCoroutine(RoundTimerLoop());
    }

    IEnumerator RoundTimerLoop()
    {
        while (true)
        {
            if (!isGameActive) yield break;

            if (CurrentState == GameState.Playing && !bossActive)
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
        if (roundDurations == null || roundDurations.Length == 0) return 30f;

        int index = Mathf.Clamp(currentRound - 1, 0, roundDurations.Length - 1);
        return Mathf.Max(1f, roundDurations[index]);
    }

    void OnRoundTimerExpired()
    {
        if (!isGameActive) return;

        if (isBossLevel && currentRound == maxRounds)
        {
            SpawnBoss();
            return;
        }

        RoundComplete();
    }

    void StopSpawners()
    {
        foreach (missile_spawn spawner in FindObjectsByType<missile_spawn>(FindObjectsSortMode.None))
        {
            spawner.CancelInvoke();
            spawner.enabled = false;
        }

        foreach (Cloud_Spawn spawner in FindObjectsByType<Cloud_Spawn>(FindObjectsSortMode.None))
        {
            spawner.CancelInvoke();
            spawner.enabled = false;
        }

        foreach (Powerup_Spawn spawner in FindObjectsByType<Powerup_Spawn>(FindObjectsSortMode.None))
        {
            spawner.CancelInvoke();
            spawner.enabled = false;
        }

        foreach (Airplane_Spawn spawner in FindObjectsByType<Airplane_Spawn>(FindObjectsSortMode.None))
        {
            spawner.CancelInvoke();
            spawner.enabled = false;
        }
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

        // 2. Reduce Missile Count
        currentMissilesLeft--;

        UpdateUI();

        // 3. Check for Win
        if (currentMissilesLeft <= 0 && !bossActive)
        {
            if (isBossLevel && currentRound == maxRounds)
            {
                SpawnBoss();
            }
            else
            {
                RoundComplete(); 
            }
        }
    }

    void SpawnBoss()
    {
        bossActive = true;
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
        PlayerProgress.UnlockBossSkin(); // call when boss dies
        RoundComplete(); // This will trigger the "Level Complete" screen
    }

    public void PlayerDied()
    {
        isGameActive = false;
        currentLives--; // Lose of life
        SetState(GameState.GameOver); // player death
        //centerText.text = "GAME OVER";
        //centerText.color = Color.red;
        CancelInvoke("AddSurvivalPoints"); // Stop giving points

        if (roundTimerRoutine != null)
        {
            StopCoroutine(roundTimerRoutine);
            roundTimerRoutine = null;
        }

        //StopSpawners();

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
        scoreKeeper = currentScore; // Save score
        SetState(GameState.GameOver); // Use GameOver state to pause time and show menus
        CancelInvoke("AddSurvivalPoints");

        if (roundTimerRoutine != null)
        {
            StopCoroutine(roundTimerRoutine);
            roundTimerRoutine = null;
        }

        StopSpawners();

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

            // unlock skins based on level 
            PlayerProgress.CompleteLevel(currentLevelNumber);

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
        else if (buttonAction == "DEPLOY BACKUP")
        {
            // Hide the menus
            restartButton.SetActive(false);
            mainmenuButton.SetActive(false);
            centerText.text = "";

            // Spawn a new jet into the current fight
            Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);

            // Resume the game logic
            isGameActive = true;
            SetState(GameState.Playing);

            // Start the point timer again
            InvokeRepeating("AddSurvivalPoints", 1f, 1f);

            // Unfreeze the round timer
            roundTimerRoutine = StartCoroutine(RoundTimerLoop());
        }
        else
        {
        // This reloads the current scene (resets everything)
        // "RESTART CAMPAIGN" still fully reloads the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
        missileText.text = "Missiles Left: " + Mathf.Max(0, currentMissilesLeft);

        if (timeText != null)
        {
            int seconds = Mathf.CeilToInt(RoundTimeRemaining);
            timeText.text = "Time: " + seconds;
        }

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
            isGameActive = true;  // makes sure missile spawner loads
            SetState(GameState.Playing);
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

        // Showcase Hard Reset
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Wipe all progress and return to main menu
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            // Reset the live session variables
            scoreKeeper = 0;
            currentLives = 3;
            currentRound = 1;

            // Unpause the game (in case they pressed L while on the Game Over screen)
            Time.timeScale = 1f;

            // Force the game to reload the Main Menu immediately
            SceneManager.LoadScene("MainMenu");

            Debug.Log("SHOWCASE WIPE COMPLETE. Game completely reset.");
        }
    }

public void SetState(GameState newState)
    {
        CurrentState = newState;

        // Control time based on state
        Time.timeScale = (newState == GameState.Paused || newState == GameState.GameOver) ? 0f : 1f;

        //Debug.Log("State changed to: " + newState);

        OnGameStateChanged?.Invoke(newState);
    }

    public void PauseGame() => SetState(GameState.Paused);
    public void ResumeGame() => SetState(GameState.Playing);

    public void GoToMainMenu()
    {
        // NEW: SAVE BEFORE LEAVING
        // We save the exact name of the scene we are currently playing in
        string currentSceneName = SceneManager.GetActiveScene().name;
        GameSettings.SaveProgress(currentSceneName, currentRound, currentLives, scoreKeeper);

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1Intro");
    }
}
