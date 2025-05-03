using System;
using UnityEngine;
using UnityEngine.Events;

public class SpeedrunTimer : MonoBehaviour
{
    private static SpeedrunTimer _instance;
    
    public float TimeElapsed { get; private set; }

    public bool IsRunning { get; private set; }

    public UnityEvent<float> onTimerUpdate;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(transform.root);
        }
    }

    private void Start()
    {
        // Debug.Log("SpeedrunTimer started, timerEnabled: " + SpeedrunTimerState.timerEnabled);
        // if (!SpeedrunTimerState.timerEnabled)
        // {
        //     gameObject.SetActive(false);
        //     return;
        // }
        
        IsRunning = false;
        DontDestroyOnLoad(gameObject);
    }

    public void OnCutsceneFinished(CutscenePlayer.CutsceneIndex index)
    {
        if (!SpeedrunTimerState.timerEnabled) return;
        var wasIntroCutscene = index.Equals(CutscenePlayer.CutsceneIndex.Intro);
        if (!wasIntroCutscene) return;
        
        IsRunning = true;
        
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Reset()
    {
        TimeElapsed = 0;
        Stop();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (!SpeedrunTimerState.timerEnabled) return;
        if (!IsRunning) return;
        if (Mathf.Approximately(Time.timeScale, 0.0f)) return;
        
        TimeElapsed += Time.deltaTime;
        
        onTimerUpdate?.Invoke(TimeElapsed);
    }
}