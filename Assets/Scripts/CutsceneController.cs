using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [Header("Scene")]
    public string nextSceneName;

    [Header("UI")]
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] lines;

    private int currentLineIndex;

    void Start()
    {
        currentLineIndex = 0;
        ShowCurrentLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Advance();
        }
    }

    public void Advance()
    {
        currentLineIndex++;

        if (lines == null || currentLineIndex >= lines.Length)
        {
            EndCutscene();
            return;
        }

        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        if (dialogueText == null) return;

        if (lines == null || lines.Length == 0)
        {
            dialogueText.text = "";
            return;
        }

        currentLineIndex = Mathf.Clamp(currentLineIndex, 0, lines.Length - 1);
        dialogueText.text = lines[currentLineIndex];
    }

    void EndCutscene()
    {
        if (string.IsNullOrWhiteSpace(nextSceneName)) return;
        SceneManager.LoadScene(nextSceneName);
    }
}
