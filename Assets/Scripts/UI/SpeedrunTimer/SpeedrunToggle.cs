using UnityEngine;
using UnityEngine.UI;

public class SpeedrunToggle : MonoBehaviour
{
    public Toggle toggle;
    
    private void Start()
    {
        toggle.isOn = SpeedrunTimerState.timerEnabled;
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }
    
    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    private static void OnToggleChanged(bool value)
    {
        SpeedrunTimerState.timerEnabled = value;
    }
    
}
