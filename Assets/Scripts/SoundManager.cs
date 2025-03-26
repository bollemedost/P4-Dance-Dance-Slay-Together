using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource audioSource;
    public AudioClip soundPerfect;

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

    public void PlaySound()
    {
        if (soundPerfect != null && !audioSource.isPlaying)  // Prevent overlapping
        {
            audioSource.PlayOneShot(soundPerfect);
        }
    }
}