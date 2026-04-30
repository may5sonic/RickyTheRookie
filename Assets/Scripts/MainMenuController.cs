using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [Header("UI Panels")]
    public GameObject customizationPanel;

    void Start()
    {
        // Load saved settings on app start
        GameSettings.Load();

        if (customizationPanel != null)
        {
            customizationPanel.SetActive(false);
        }
    }

    // Play Button Logic
    public void PlayGame()
    {
        // Check the hard drive for a saved game
        GameSettings.LoadProgress();

        // If the SavedLevel is NOT empty, we have a save! Resume it.
        if (GameSettings.SavedLevel != "")
        {
            GameManager.currentRound = GameSettings.SavedRound;
            GameManager.currentLives = GameSettings.SavedLives;
            GameManager.scoreKeeper = GameSettings.SavedScore;

            SceneManager.LoadScene(GameSettings.SavedLevel);
        }

        // Otherwise, there is no save. Start a brand new game.
    else
        {
            GameSettings.ClearProgress(); // Ensure memory is completely wiped
            GameManager.scoreKeeper = 0;
            GameManager.currentLives = 3;
            GameManager.currentRound = 1;

            SceneManager.LoadScene("Level1Intro");
        }
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void OpenCustomization()
    {
        customizationPanel.SetActive(true);
    }

    // close customization panel
    public void CloseCustomization()
    {
        customizationPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit(); // Works in build, not in editor
    }

    void Update()
    {
        // Secret developer button to wipe saves during testing
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameSettings.ClearProgress();
            Debug.Log("SAVE WIPED! Next time you press Play, the Intro will run.");
        }
    }
}
