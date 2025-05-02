using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{

    [SerializeField] private CutscenePlayer player;
    [SerializeField] private CutscenePlayer.CutsceneIndex sceneToPlay;
    
    private async void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            await player.PlayScene(sceneToPlay);
        }
    }
}
