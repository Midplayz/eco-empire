using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [field: SerializeField] private AudioClip backgroundMusic;
    [field: SerializeField] private AudioClip smallTruckPurchase;
    [field: SerializeField] private AudioClip largeTruckPurchase;
    [field: SerializeField] private AudioClip purchaseSound;

    [field: SerializeField] private AudioSource backgroundMusicAudioSource;
    [field: SerializeField] private AudioSource sfxAudioSource;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        backgroundMusicAudioSource.volume = 0.5f;
        backgroundMusicAudioSource.clip = backgroundMusic;
        backgroundMusicAudioSource.loop = true;
        backgroundMusicAudioSource.Play();
    }

    public void PlaySmallTruckPurchaseSound()
    {
        sfxAudioSource.clip = smallTruckPurchase;
        sfxAudioSource.loop = false;
        sfxAudioSource.Play();
    }
    public void PlaylargeTruckPurchaseSound()
    {
        sfxAudioSource.clip = largeTruckPurchase;
        sfxAudioSource.loop = false;
        sfxAudioSource.Play();
    }
    public void PlayPurchaseSound()
    {
        sfxAudioSource.clip = purchaseSound;
        sfxAudioSource.loop = false;
        sfxAudioSource.Play();
    }
}
