using UnityEngine;

public class SpeedrunTimerCleanup : MonoBehaviour
{
    private SpeedrunTimer _speedrunTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _speedrunTimer = FindFirstObjectByType<SpeedrunTimer>();
        if (_speedrunTimer == null) return;
    }

    public void ResetTimer()
    {
        if (_speedrunTimer == null) return;
        // Destroy(_speedrunTimer);
        _speedrunTimer.Reset();
    }

    public void StopTimer()
    {
        if (_speedrunTimer == null) return;
        _speedrunTimer.Stop();
    }

    public void OnCutsceneFinished(CutscenePlayer.CutsceneIndex index)
    {
        if (_speedrunTimer == null) return;
        _speedrunTimer.OnCutsceneFinished(index);
    }
    
}
