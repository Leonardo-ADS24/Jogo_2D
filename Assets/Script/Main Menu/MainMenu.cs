using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;

    public void Play()
    {
        _sceneController.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
