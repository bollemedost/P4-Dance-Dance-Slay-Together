using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource audioSource;
    public AudioClip soundPerfect;
    public AudioClip soundMiss;
    public AudioClip soundBoom;
    public AudioClip soundGoodJob;
    public AudioClip soundKeepSlaying;
    public AudioClip soundSwooshBling;
    public AudioClip soundYouGotThis;

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
            audioSource.PlayOneShot(soundPerfect);
        }
    }

    public void PlayMissSound()
    {
        if (soundMiss != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundMiss);
        }
    }

    public void PlayBoomSound()
    {
        if (soundBoom != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundBoom);
        }
    }

    public void PlayGoodJobSound()
    {
        if (soundGoodJob != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundGoodJob);
        }
    }

    public void PlayKeepSlaying()
    {
        if (soundKeepSlaying != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundKeepSlaying);
        }
    }

    public void PlaySwooshBlingSlaying()
    {
        if (soundSwooshBling != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundSwooshBling);
        }
    }

    public void PlayYouGotThis()
    {
        if (soundYouGotThis != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundYouGotThis);
        }
    }
}