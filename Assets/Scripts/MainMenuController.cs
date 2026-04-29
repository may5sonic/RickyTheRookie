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

        if (customizationPanel != null) {
            customizationPanel.SetActive(false);
        }
    }

    public void PlayGame()
    {
        GameManager.scoreKeeper = 0;
        GameManager.currentLives = 3;
        GameManager.currentRound = 1;
        SceneManager.LoadScene("Level1Intro");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void OpenCustomization() {
        customizationPanel.SetActive(true);
    }

    // close customization panel
     public void CloseCustomization() {
        customizationPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit(); // Works in build, not in editor
    }
}
