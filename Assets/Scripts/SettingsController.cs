using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("Player Settings")]
    public Slider speedSlider;
    public TextMeshProUGUI speedValueText;

    public float minSpeed = 1f;
    public float maxSpeed = 12f;

    [Header("Bullet Settings")]
    public Slider bulletSlider;       // Drag your NEW slider here
    public TextMeshProUGUI bulletText; // Drag your NEW text here
    public float minBulletSpeed = 10f;
    public float maxBulletSpeed = 40f;

    void Start()
    {
        GameSettings.Load();

        speedSlider.minValue = minSpeed;
        speedSlider.maxValue = maxSpeed;

        speedSlider.value = GameSettings.PlayerSpeed;
        UpdateText(speedSlider.value);

        speedSlider.onValueChanged.AddListener(OnSpeedChanged);

        if (bulletSlider != null)
        {
            bulletSlider.minValue = minBulletSpeed;
            bulletSlider.maxValue = maxBulletSpeed;
            bulletSlider.value = GameSettings.BulletSpeed;
            UpdateBulletText(bulletSlider.value);
            bulletSlider.onValueChanged.AddListener(OnBulletSpeedChanged);
        }
    }

    public void OnSpeedChanged(float newSpeed)
    {
        GameSettings.PlayerSpeed = newSpeed;
        GameSettings.Save();
        UpdateText(newSpeed);
    }

    public void OnBulletSpeedChanged(float newSpeed)
    {
        GameSettings.BulletSpeed = newSpeed;
        GameSettings.Save();
        UpdateBulletText(newSpeed);
    }

    void UpdateText(float value)
    {
        if (speedValueText != null)
            speedValueText.text = $"Speed: {value:0.0}";
    }

    void UpdateBulletText(float value)
    {
        if (bulletText != null) bulletText.text = $"Bullet Speed: {value:0.0}";
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
