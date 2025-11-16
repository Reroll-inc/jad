using UnityEngine;

public class Credits : MonoBehaviour
{
    public void BackToMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
