
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string aquariumSceneName = "aquarium";

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Start()
    {
        ShowMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(aquariumSceneName);
    }

    public void ShowCredits()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    
public void QuitGame()
{
    Debug.Log("Quitting AquaDiscover...");

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
}

}
