using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TriInspector;

public class MainMenuButtons: MonoBehaviour
{
    [SerializeField] [Scene] private string _loadGame = "MainGame";
    [SerializeField] private Image _sceneFade;
    [SerializeField] private float _fadeSpeed = 1f;
    [SerializeField] private AudioSource _menuMusic;
    [SerializeField] private AudioSource _buttonMusic;
    protected void Start()
    {
        _sceneFade.enabled = false;
    }

    public void LoadGame()
    {
        _sceneFade.enabled = true;
        _menuMusic.enabled = false;
        _buttonMusic.Play();
        StartCoroutine(Fade());
        Invoke("Load", _fadeSpeed);
    }
    protected void Load()
    {
        SceneManager.LoadScene(_loadGame);
    }
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    IEnumerator Fade()
    {
        Color endColor = new Color(0, 0, 0, 1);
        Color sourceColor = new Color(0, 0, 0, 0);
        float timer = 0;
        while (timer <=_fadeSpeed)
        {
            _sceneFade.color = Color.Lerp(sourceColor, endColor, timer/_fadeSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
        _sceneFade.color = endColor;
    }
}

