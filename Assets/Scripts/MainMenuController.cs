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
        SceneManager.LoadScene("Game");
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
