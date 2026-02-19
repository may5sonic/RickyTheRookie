using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    // Drag the PausePanel here from the Hierarchy
    public GameObject pausePanel;

    void Awake()
    {
        // Ensure the pause panel starts hidden
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void OnEnable()
    {
        // Subscribe to state change event
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        // Unsubscribe when object is disabled/destroyed
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    // This method runs whenever GameManager changes state
    void HandleGameStateChanged(GameManager.GameState state)
    {
        // Show the pause panel only when paused
        if (pausePanel != null)
            pausePanel.SetActive(state == GameManager.GameState.Paused);
    }

    // Called by Resume button
    public void Resume()
    {
        GameManager.Instance.ResumeGame();
    }

    // Called by Main Menu button
    public void GoToMainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }
}