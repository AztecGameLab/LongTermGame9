using System;
using UnityEngine;
using UnityEngine.UI;

public class SpeedrunTimerDisplay : MonoBehaviour
{
    [SerializeField] private Text timerText;

    private SpeedrunTimer _speedrunTimer;
    
    private void Start()
    {
        Debug.Log("SpeedrunTimerDisplay started, timerEnabled: " + SpeedrunTimerState.timerEnabled);
        if (!SpeedrunTimerState.timerEnabled)
        {
            gameObject.SetActive(false);
            return;
        }
        
        _speedrunTimer = FindFirstObjectByType<SpeedrunTimer>();
        if (_speedrunTimer == null) return;
        
        UpdateTimer(_speedrunTimer.TimeElapsed);
        _speedrunTimer.onTimerUpdate.AddListener(UpdateTimer);
    }

    private void OnDestroy()
    {
        if (_speedrunTimer == null) return;
        _speedrunTimer.onTimerUpdate.RemoveListener(UpdateTimer);
    }

    private void UpdateTimer(float time)
    {
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = Mathf.FloorToInt(time % 60);
        var milliseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);
        
        timerText.text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D2}";
    }
}
