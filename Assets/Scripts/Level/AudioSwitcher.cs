using UnityEngine;

public class AudioSwitcher : MonoBehaviour
{

    public AudioSource levelMusic;
    public AudioSource bossMusic;
    public AudioSource desertAmbience;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //bossMusic.gameObject.SetActive(false);
        //levelMusic.gameObject.SetActive(true);
    }

    public void triggerBoss(bool val)
    {
        bossMusic.gameObject.SetActive(val);
    }

    public void triggerDesert(bool val)
    {
        levelMusic.gameObject.SetActive(val);
    }

    public void triggerBackgroundAudio()
    {
        bossMusic.gameObject.SetActive(false);
        levelMusic.gameObject.SetActive(true);
        desertAmbience.gameObject.SetActive(true);
    }
}
