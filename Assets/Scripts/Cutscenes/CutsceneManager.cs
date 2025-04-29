using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Cutscenes
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private string defaultVideoPath = "Videos/kyu-kurarin.webm";
        private string _loadGame;
        
        private void Awake()
        {
            _loadGame = MasterScript.MainGame;
        }

        private void Start()
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            var filePath = GetVideoPath();
            PlayVideo(filePath);
        }

        private void OnVideoFinished(VideoPlayer vp)
        {
            LoadScene();
        }   

        public void LoadScene()
        {
            SceneManager.LoadScene(_loadGame);
        }

        private string GetVideoPath()
        {
            // this might not work on Android
            // see: https://docs.unity3d.com/6000.0/Documentation/Manual/StreamingAssets.html
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, defaultVideoPath);
            return filePath;
        }

        public void PlayVideo(string filePath)
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = filePath;
            videoPlayer.Prepare();
        }
    }
}