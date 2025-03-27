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
        if (soundPerfect != null)
        {
            AudioSource.PlayClipAtPoint(soundPerfect, Vector3.zero, volumePerfect);
        }
    }

    public void PlayMissSound()
    {
        if (soundMiss != null)
        {
            AudioSource.PlayClipAtPoint(soundMiss, Vector3.zero, volumeMiss);
        }
    }

    public void PlayBoomSound()
    {
        if (soundBoom != null)
        {
            AudioSource.PlayClipAtPoint(soundBoom, Vector3.zero, volumeBoom);
        }
    }

    public void PlayGoodJobSound()
    {
        if (soundGoodJob != null)
        {
            AudioSource.PlayClipAtPoint(soundGoodJob, Vector3.zero, volumeGoodJob);
        }
    }

    public void PlayKeepSlaying()
    {
        if (soundKeepSlaying != null)
        {
            AudioSource.PlayClipAtPoint(soundKeepSlaying, Vector3.zero, volumeKeepSlaying);
        }
    }

    public void PlaySwooshBlingSlaying()
    {
        if (soundSwooshBling != null)
        {
            AudioSource.PlayClipAtPoint(soundSwooshBling, Vector3.zero, volumeSwooshBling);
        }
    }

    public void PlayYouGotThis()
    {
        if (soundYouGotThis != null)
        {
            AudioSource.PlayClipAtPoint(soundYouGotThis, Vector3.zero, volumeYouGotThis);
        }
    }
}
