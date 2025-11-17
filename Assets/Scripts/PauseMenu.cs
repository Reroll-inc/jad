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
        GameManager.Instance.PlayUICancelSound();
        ResumeGame();
    }

    public void OnPauseButtonPressed()
    {
        GameManager.Instance.PlayUIConfirmSound();
        ShowMainPausePanel();
    }

    public void OnOptionsButtonPressed()
    {
        GameManager.Instance.PlayUIConfirmSound();
        ShowOptionsMenu();
    }

    public void OpenControllerPanel()
    {
        GameManager.Instance.PlayUIConfirmSound();
        controllerPanel.SetActive(true);
        soundPanel.SetActive(false);
        graphicsPanel.SetActive(false);
    }
    public void OpenSoundPanel()
    {
        GameManager.Instance.PlayUIConfirmSound();
        controllerPanel.SetActive(false);
        soundPanel.SetActive(true);
        graphicsPanel.SetActive(false);
    }
    public void OpenGraphicsPanel()
    {
        GameManager.Instance.PlayUIConfirmSound();
        controllerPanel.SetActive(false);
        soundPanel.SetActive(false);
        graphicsPanel.SetActive(true);
    }

    public void OnQuitToMenuButtonPressed()
    {
        GameManager.Instance.PlayUIConfirmSound();
        LevelManager.Instance.GoBackToMenu();
    }

    public void OnQuitGameButtonPressed()
    {
        GameManager.Instance.PlayUIConfirmSound();
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
        GameManager.Instance.PlayUIConfirmSound();
        mainPausePanel.SetActive(false);
        warningMenuPanel.SetActive(true);
    }

    public void WarningQuitGame()
    {
        GameManager.Instance.PlayUIConfirmSound();
        mainPausePanel.SetActive(false);
        warningQuitPanel.SetActive(true);
    }
}
