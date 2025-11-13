using UnityEngine;

public enum SubMenu
{
    CONTROLLER,
    SOUND,
    GRAPHICS
}

public class MainMenu : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;

    [Header("Options Menu")]
    [SerializeField] private GameObject mainOptionsMenu;
    [SerializeField] private GameObject controllerMenu;
    [SerializeField] private GameObject soundMenu;
    [SerializeField] private GameObject graphicsMenu;

    void Start()
    {
        OpenMainMenuPanel();
        OpenSubMenu(null);
    }
    void OpenMainMenuPanel()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    void OpenSubMenu(SubMenu? subMenu)
    {
        controllerMenu.SetActive(SubMenu.CONTROLLER == subMenu);
        soundMenu.SetActive(SubMenu.SOUND == subMenu);
        graphicsMenu.SetActive(SubMenu.GRAPHICS == subMenu);
    }

    public void OpenOptionsPanel()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

        mainOptionsMenu.SetActive(true);

        OpenSubMenu(SubMenu.CONTROLLER);
    }

    public void OpenCreditsPanel()
    {
        GameManager.Instance.LoadCredits();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void PlayGame()
    {
        GameManager.Instance.StartGame();
    }
}
