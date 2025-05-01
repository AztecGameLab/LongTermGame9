using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject saguaro;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void restart()
    {
        // var currentScene = SceneManager.GetSceneByPath(MasterScript.MainGame);
        var currentScene = SceneManager.GetActiveScene();
        saguaro.SetActive(false);
        panel.SetActive(!panel.activeSelf);
        SceneManager.LoadScene(currentScene.buildIndex);
        Time.timeScale = 1;
        //SceneManager.UnloadSceneAsync(currentScene);
    }
    
    public void mainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void pause()
    {
        panel.SetActive(!panel.activeSelf);
        Time.timeScale = (int)Time.timeScale == 1 ? 0 : 1;
    }
    
    public void pause(InputAction.CallbackContext _) { pause(); }
}
