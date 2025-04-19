using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{

    [SerializeField] private GameObject panel;

    private void Start()
    {
        panel.SetActive(false);
    }
    
    public void mainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void pause()
    {
        panel.SetActive(!panel.activeSelf);
        Time.timeScale = (int)(Time.timeScale) == 1 ? 0 : 1;
    }
    
    public void pause(InputAction.CallbackContext _)
    {
        pause();
    }
}
