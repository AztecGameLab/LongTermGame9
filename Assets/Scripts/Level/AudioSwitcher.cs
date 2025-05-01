using UnityEngine;

public class AudioSwitcher : MonoBehaviour
{

    public AudioSource levelMusic;
    public AudioSource bossMusic;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossMusic.gameObject.SetActive(false);
        levelMusic.gameObject.SetActive(true);
    }

    public void triggerBoss()
    {
        bossMusic.gameObject.SetActive(true);
        levelMusic.gameObject.SetActive(false);
    }
}
