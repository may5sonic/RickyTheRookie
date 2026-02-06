using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Needed for TextMeshPro
using UnityEngine.SceneManagement; // Needed if you want to restart later

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int scoreKeeper = 0;

    [Header("UI Settings")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI missileText;
    public TextMeshProUGUI centerText;
    public GameObject restartButton;

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
            instance = this;
        else
            Destroy(gameObject);
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

    public void GameOver()
    {
        isGameActive = false;
        centerText.text = "GAME OVER";
        centerText.color = Color.red;
        restartButton.SetActive(true);
        CancelInvoke("AddSurvivalPoints"); // Stop giving points

        scoreKeeper = 0;
    }

    void WinGame()
    {
        isGameActive = false;
        centerText.text = "YOU WIN PILOT";
        centerText.color = Color.green;
        restartButton.SetActive(true);
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
}