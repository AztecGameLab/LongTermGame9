using UnityEngine;
using UnityEngine.Serialization;

public class SnakeSfx : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip snakeAttackSfx;
    [SerializeField] private float attackVolume = 1f;
    [SerializeField] private AudioClip snakeRattleSfx;
    [SerializeField] private float rattleVolume = 1f;
    
    public void PlayAttack()
    {
        audioSource.PlayOneShot(snakeAttackSfx, attackVolume);
    }
    
    public void PlayRattle()
    {
        audioSource.PlayOneShot(snakeRattleSfx, rattleVolume);
    }
    
}
