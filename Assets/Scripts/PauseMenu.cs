using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu panels")]
    [SerializeField] private GameObject mainPausePanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject controllerPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject graphicsPanel;
    [SerializeField] private GameObject warningMenuPanel;
    [SerializeField] private GameObject warningQuitPanel;

    void OpenSubMenu(SubMenu? subMenu)
    {
        controllerPanel.SetActive(SubMenu.CONTROLLER == subMenu);
        soundPanel.SetActive(SubMenu.SOUND == subMenu);
        graphicsPanel.SetActive(SubMenu.GRAPHICS == subMenu);
    }

    public void OnResumeButtonPressed()
    {
        ResumeGame();
    }

    public void OnPauseButtonPressed()
    {
        ShowMainPausePanel();
    }

    public void OnOptionsButtonPressed()
    {
        ShowOptionsMenu();
    }

    public void OpenControllerPanel()
    {
        controllerPanel.SetActive(true);
        soundPanel.SetActive(false);
        graphicsPanel.SetActive(false);
    }
    public void OpenSoundPanel()
    {
        controllerPanel.SetActive(false);
        soundPanel.SetActive(true);
        graphicsPanel.SetActive(false);
    }
    public void OpenGraphicsPanel()
    {
        controllerPanel.SetActive(false);
        soundPanel.SetActive(false);
        graphicsPanel.SetActive(true);
    }

    public void OnQuitToMenuButtonPressed()
    {
        LevelManager.Instance.GoBackToMenu();
    }

    public void OnQuitGameButtonPressed()
    {
        GameManager.Instance.QuitGame();
    }

    void ResumeGame()
    {
        mainPausePanel.SetActive(false);
        LevelManager.Instance.StartLevel();
    }

    void ShowMainPausePanel()
    {
        mainPausePanel.SetActive(true);
        optionsMenuPanel.SetActive(false);
        warningMenuPanel.SetActive(false);
        warningQuitPanel.SetActive(false);
        //OpenSubMenu(null);
    }

    void ShowOptionsMenu()
    {
        mainPausePanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
        OpenSubMenu(SubMenu.CONTROLLER);
    }

    public void WarningBackToMenu()
    {
        mainPausePanel.SetActive(false);
        warningMenuPanel.SetActive(true);
    }

    public void WarningQuitGame()
    {
        mainPausePanel.SetActive(false);
        warningQuitPanel.SetActive(true);
    }
}
