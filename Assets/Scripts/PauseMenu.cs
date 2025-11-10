using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu panels")]
    [SerializeField] private GameObject mainPausePanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject controllerPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject graphicsPanel;

    void OnEnable()
    {
        ShowMainPausePanel();
    }
    public void OnResumeButtonPressed() // pause/resume game
    {
        GameManager.Instance.TogglePause();
    }

    public void OnOptionsButtonPressed() // Show options in-game menu
    {
        ShowOptionsMenu();
    }

    public void OnQuitGameButtonPressed() // Quit game
    {
        GameManager.Instance.QuitGame();
    }

    public void OnControllerButtonPressed() // Open controller panel
    {
        optionsMenuPanel.SetActive(false);
        controllerPanel.SetActive(true);
    }

    public void OnSoundButtonPressed() // Open sound settings
    {
        optionsMenuPanel.SetActive(false);
        soundPanel.SetActive(true);
    }

    public void OnGraphicsButtonPressed() // Open graphics settings
    {
        optionsMenuPanel.SetActive(false);
        graphicsPanel.SetActive(true);
    }

    public void OnQuitToMenuButtonPressed() // Exit to main menu
    {
        GameManager.Instance.QuitToMenu();
    }

    public void OnOptionsBackButtonPressed()
    {
        ShowMainPausePanel();
    }

    public void OnSubMenuBackButtonPressed()
    {
        ShowOptionsMenu();
    }

    private void ShowMainPausePanel()
    {
        mainPausePanel.SetActive(true);
        optionsMenuPanel.SetActive(false);
        controllerPanel.SetActive(false);
        soundPanel.SetActive(false);
        graphicsPanel.SetActive(false);
    }

    private void ShowOptionsMenu()
    {
        mainPausePanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
        controllerPanel.SetActive(false);
        soundPanel.SetActive(false);
        graphicsPanel.SetActive(false);
    }
}
