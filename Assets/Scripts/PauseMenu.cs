using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu panels")]
    [SerializeField] private GameObject mainPausePanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject controllerPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject graphicsPanel;

    void OpenSubMenu(SubMenu? subMenu)
    {
        controllerPanel.SetActive(SubMenu.CONTROLLER == subMenu);
        soundPanel.SetActive(SubMenu.SOUND == subMenu);
        graphicsPanel.SetActive(SubMenu.GRAPHICS == subMenu);
    }

    public void OnResumeButtonPressed()
    {
        GameManager.Instance.TogglePause();
    }

    public void OnOptionsButtonPressed()
    {
        ShowOptionsMenu();
    }

    public void OnQuitGameButtonPressed()
    {
        GameManager.Instance.QuitGame();
    }

    public void OnControllerButtonPressed()
    {
        optionsMenuPanel.SetActive(false);
        OpenSubMenu(SubMenu.CONTROLLER);
    }

    public void OnSoundButtonPressed()
    {
        optionsMenuPanel.SetActive(false);
        OpenSubMenu(SubMenu.SOUND);
    }

    public void OnGraphicsButtonPressed()
    {
        optionsMenuPanel.SetActive(false);
        OpenSubMenu(SubMenu.GRAPHICS);
    }

    public void OnQuitToMenuButtonPressed()
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
        OpenSubMenu(null);
    }

    private void ShowOptionsMenu()
    {
        mainPausePanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
        OpenSubMenu(null);
    }
}
