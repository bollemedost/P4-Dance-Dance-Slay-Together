using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource audioSource;

    [Header("Sound Clips")]
    public AudioClip soundPerfect;
    public AudioClip soundMiss;
    public AudioClip soundBoom;
    public AudioClip soundGoodJob;
    public AudioClip soundKeepSlaying;
    public AudioClip soundSwooshBling;
    public AudioClip soundYouGotThis;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float volumePerfect = 1f;
    [Range(0f, 1f)] public float volumeMiss = 1f;
    [Range(0f, 1f)] public float volumeBoom = 1f;
    [Range(0f, 1f)] public float volumeGoodJob = 1f;
    [Range(0f, 1f)] public float volumeKeepSlaying = 1f;
    [Range(0f, 1f)] public float volumeSwooshBling = 1f;
    [Range(0f, 1f)] public float volumeYouGotThis = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPerfectSound()
    {
        if (soundPerfect != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundPerfect, volumePerfect);
        }
    }

    public void PlayMissSound()
    {
        if (soundMiss != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundMiss, volumeMiss);
        }
    }

    public void PlayBoomSound()
    {
        if (soundBoom != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundBoom, volumeBoom);
        }
    }

    public void PlayGoodJobSound()
    {
        if (soundGoodJob != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundGoodJob, volumeGoodJob);
        }
    }

    public void PlayKeepSlaying()
    {
        if (soundKeepSlaying != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundKeepSlaying, volumeKeepSlaying);
        }
    }

    public void PlaySwooshBlingSlaying()
    {
        if (soundSwooshBling != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundSwooshBling, volumeSwooshBling);
        }
    }

    public void PlayYouGotThis()
    {
        if (soundYouGotThis != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundYouGotThis, volumeYouGotThis);
        }
    }
}
