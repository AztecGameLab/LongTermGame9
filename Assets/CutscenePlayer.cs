using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[HideMonoScript]
public class CutscenePlayer : MonoBehaviour
{
    
    [SerializeField] private List<VideoClip> videos = new List<VideoClip>();
    private CutsceneIndex _videoIndex;
    private VideoPlayer player;
    [SerializeField] private RawImage videoRenderer;
    [SerializeField] private GameObject button;
    [SerializeField] private Image fadeOut;
    [SerializeField] private int fadeOutTime;
    [SerializeField] private AudioSwitcher audioManager;
    [SerializeField] private GameObject SaguaroSounds;
    
    public UnityEvent<CutsceneIndex> onCutsceneFinished;

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
        fadeOut.CrossFadeAlpha(1, 0, true);
        await PlayScene(CutsceneIndex.Intro);
        audioManager.triggerBackgroundAudio();
    }

    public async UniTask PlayScene(CutsceneIndex index)
    {
        // Stop Everything
        Time.timeScale = 0; 
        SaguaroSounds.SetActive(false);
        // FadeOut
        fadeOut.enabled = true;
        fadeOut.CrossFadeAlpha(1, fadeOutTime, true);
        await UniTask.Delay(TimeSpan.FromSeconds(fadeOutTime), true);
        // Set up Player
        player.clip = videos[(int)index];
        ToggleVideo();
        player.Play(); await UniTask.WaitUntil(() => player.isPlaying);
        fadeOut.enabled = false;
        button.SetActive(true);
        await UniTask.WaitUntil(() => !player.isPlaying);
        ToggleVideo(); button.SetActive(false);
        fadeOut.CrossFadeAlpha(0, 0, true);
        // Resume Everything
        SaguaroSounds.SetActive(true);
        Time.timeScale = 1;
        onCutsceneFinished?.Invoke(index);
    }

    public void SkipScene() { player.Stop(); }

    private void ToggleVideo()
    {
        player.enabled = !player.enabled;
        videoRenderer.enabled = !videoRenderer.enabled;
    }

    public async void BossScene()
    {
        audioManager.triggerDesert(false);
        await PlayScene(CutsceneIndex.PreBoss);
        audioManager.triggerBoss(true);
    }

    public async void BossDefeatScene()
    {
        audioManager.triggerBoss(false);
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        await PlayScene(CutsceneIndex.PostBoss);
        SceneManager.LoadScene("WinScene");
    }
    
    // Update is called once per frame
}
