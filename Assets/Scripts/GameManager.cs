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


    public static int scoreKeeper = 0;

    [Header("UI Settings")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI missileText;
    public TextMeshProUGUI centerText;
    public GameObject restartButton;
    public GameObject mainmenuButton;

    [Header("Game Settings")]
    public int missilesToWin = 10;
    public int scorePerMissile = 100;

    private int currentScore = 0;
    private int currentMissilesLeft;
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

        currentScore = scoreKeeper;

        currentMissilesLeft = missilesToWin;
        UpdateUI();

        // Start adding survival points every 1 second
        InvokeRepeating("AddSurvivalPoints", 1f, 1f);
    }

    void AddSurvivalPoints()
    {
        if (isGameActive)
        {
            currentScore += 1; // +1 point for every second you survive
            UpdateUI();
        }
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
        if (currentMissilesLeft <= 0)
        {
            WinGame();
        }
    }

    public void PlayerDied()
    {
        isGameActive = false;
        SetState(GameState.GameOver); // player death
        centerText.text = "GAME OVER";
        centerText.color = Color.red;
        restartButton.SetActive(true);
        mainmenuButton.SetActive(true);
        CancelInvoke("AddSurvivalPoints"); // Stop giving points

        scoreKeeper = 0;
    }

    void WinGame()
    {
        isGameActive = false;
        centerText.text = "YOU WIN PILOT";
        centerText.color = Color.green;
        restartButton.SetActive(true);
        mainmenuButton.SetActive(true);
        CancelInvoke("AddSurvivalPoints");
        scoreKeeper = currentScore;
    }

    public void RestartGame()
    {
        // This reloads the current scene (resets everything)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
        missileText.text = "Missiles Left: " + Mathf.Max(0, currentMissilesLeft);
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
        else if (scene.name == "Game") 
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