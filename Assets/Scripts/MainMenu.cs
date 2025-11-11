using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menú Principal")]
    public GameObject mainMenu;
    public GameObject optionsMenu;

    [Header("Menú de Opciones")]
    public GameObject mainOptionsMenu;
    public GameObject controllerMenu;
    public GameObject soundMenu;
    public GameObject graphicsMenu;

    void Start()
    {
        OpenMainMenuPanel(); // Menu starts in main menu

        // Deactivate submenus at the beginning
        controllerMenu.SetActive(false);
        soundMenu.SetActive(false);
        graphicsMenu.SetActive(false);
    }
    public void OpenMainMenuPanel()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void OpenOptionsPanel()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

        mainOptionsMenu.SetActive(true);

        controllerMenu.SetActive(false);
        soundMenu.SetActive(false);
        graphicsMenu.SetActive(false);
    }

    public void OpenControllerMenu()
    {
        controllerMenu.SetActive(true);
        soundMenu.SetActive(false);
        graphicsMenu.SetActive(false);
    }

    public void OpenSoundMenu()
    {
        soundMenu.SetActive(true);
        controllerMenu.SetActive(false);
        graphicsMenu.SetActive(false);
    }

    public void OpenGraphicsMenu()
    {
        graphicsMenu.SetActive(true);
        controllerMenu.SetActive(false);
        soundMenu.SetActive(false);
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
