using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TriInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[HideMonoScript]
public class CutscenePlayer : MonoBehaviour
{
    
    [SerializeField] private List<VideoClip> videos = new List<VideoClip>();
    private CutsceneIndex _videoIndex;
    private VideoPlayer player;
    [SerializeField] private GameObject button;
    [SerializeField] private Image fadeOut;
    [SerializeField] private int fadeOutTime;

    public enum CutsceneIndex
    {
        Intro = 0,
        PreBoss = 1,
        PostBoss = 2
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f), true);
        player = GetComponent<VideoPlayer>();
        player.enabled = false; button.SetActive(false);
        await PlayScene(CutsceneIndex.Intro);
    }

    public async UniTask PlayScene(CutsceneIndex index)
    {
        // Stop Everything
        Time.timeScale = 0; 
        // FadeOut
        fadeOut.CrossFadeAlpha(0, 0, true);
        fadeOut.enabled = true;
        fadeOut.CrossFadeAlpha(1, fadeOutTime, true);
        await UniTask.Delay(TimeSpan.FromSeconds(fadeOutTime), true);
        // Set up Player
        player.clip = videos[(int)index];
        player.enabled = true; 
        player.Play(); await UniTask.WaitUntil(() => player.isPlaying);
        fadeOut.enabled = false;
        button.SetActive(true);
        await UniTask.WaitUntil(() => !player.isPlaying);
        player.enabled = false; button.SetActive(false);
        // Resume Everything
        Time.timeScale = 1;
    }

    public void SkipScene()
    {
        player.Stop();
    }
    
    // Update is called once per frame
}
