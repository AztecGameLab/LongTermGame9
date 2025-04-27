using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Cutscenes
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        private string _loadGame;

        private void Awake()
        {
            _loadGame = MasterScript.MainGame;
        }

        private void Start()
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        private void OnVideoFinished(VideoPlayer vp)
        {
            LoadScene();
        }   

        public void LoadScene()
        {
            SceneManager.LoadScene(_loadGame);
        }
    }
}