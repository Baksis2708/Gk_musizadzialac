using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    void OnStartButtonClick()
    {
        Debug.Log("Start Game");
        SceneManager.LoadScene("Kox");
    }

    void OnQuitButtonClick()
    {
        Debug.Log("Quit Game");

        if (!Application.isEditor) // Zamknij tylko w buildzie
        {
            Application.Quit();
        }
    }
}