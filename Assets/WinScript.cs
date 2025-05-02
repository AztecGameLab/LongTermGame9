using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{

    [SerializeField] private Image fade;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1), true);
        fade.CrossFadeAlpha(0, 2, true);
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        fade.enabled = false;
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
