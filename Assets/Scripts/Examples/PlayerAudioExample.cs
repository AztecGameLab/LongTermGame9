using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAudioExample : MonoBehaviour
{
    public EventReference testSound;

    public void PlaySound(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RuntimeManager.PlayOneShot(testSound);
        }
    }
}
