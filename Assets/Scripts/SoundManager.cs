using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource audioSource;

    private int lastMotivationalIndex = -1;

    [Header("Volume Settings")]
    public float volumeKeepSlaying = 3f;
    public float volumePerfect = 3f;
    public float volumeMissSound = 3f;
    public float volumeBoom = 3f;
    public float volumeGoodJob = 3f;
    public float volumeSwooshBling = 3f;
    public float volumeYouGotThis = 3f;
    public float volumeStart = 3f;
    public float volumeStarFairy = 3f;

    [Header("Sound Clips")]
    public AudioClip soundPerfect;
    public AudioClip soundMiss;
    public AudioClip soundBoom;
    public AudioClip soundGoodJob;
    public AudioClip soundKeepSlaying;
    public AudioClip soundSwooshBling;
    public AudioClip soundYouGotThis;
    public AudioClip soundStart;
    public AudioClip soundMagicFairy;

    [System.Serializable]
    private struct MotivationalSound
    {
        public AudioClip clip;
        public float volume;

        public MotivationalSound(AudioClip clip, float volume)
        {
            this.clip = clip;
            this.volume = volume;
        }
    }

    private MotivationalSound[] motivationalSounds;

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

        motivationalSounds = new MotivationalSound[]
        {
            new MotivationalSound(soundGoodJob, volumeGoodJob),
            new MotivationalSound(soundKeepSlaying, volumeKeepSlaying),
            new MotivationalSound(soundYouGotThis, volumeYouGotThis)
        };
    }

    public void PlayPerfectSound()
    {
        if (soundPerfect != null)
        {
            audioSource.PlayOneShot(soundPerfect, volumePerfect);
        }
    }

    public void PlayMissSound()
    {
        if (soundMiss != null)
        {
            audioSource.PlayOneShot(soundMiss, volumeMissSound);
        }
    }

    public void PlayBoomSound()
    {
        if (soundBoom != null)
        {
            audioSource.PlayOneShot(soundBoom, volumeBoom);
        }
    }

    public void PlayGoodJobSound()
    {
        if (soundGoodJob != null)
        {
            audioSource.PlayOneShot(soundGoodJob, volumeGoodJob);
        }
    }

    public void PlayKeepSlaying()
    {
        if (soundKeepSlaying != null)
        {
            audioSource.PlayOneShot(soundKeepSlaying, volumeKeepSlaying);
        }
    }

    public void PlayYouGotThis()
    {
        if (soundYouGotThis != null)
        {
            audioSource.PlayOneShot(soundYouGotThis, volumeYouGotThis);
        }
    }

    public void PlaySwooshBling()
    {
        if (soundSwooshBling != null)
        {
            audioSource.PlayOneShot(soundSwooshBling, volumeSwooshBling);
        }
    }

    public void PlayStart()
    {
        if (soundYouGotThis != null)
        {
            audioSource.PlayOneShot(soundStart, volumeStart);
        }
    }

    public void PlayStarFairy()
    {
        if (soundYouGotThis != null)
        {
            audioSource.PlayOneShot(soundMagicFairy, volumeStarFairy);
        }
    }

    public void PlayRandomMotivationalSound()
    {
        var motivationalSounds = new (AudioClip clip, float volume)[]
        {
        (soundGoodJob, volumeGoodJob),
        (soundKeepSlaying, volumeKeepSlaying),
        (soundYouGotThis, volumeYouGotThis)
        };

        var validSounds = new System.Collections.Generic.List<(AudioClip clip, float volume, int index)>();

        for (int i = 0; i < motivationalSounds.Length; i++)
        {
            if (motivationalSounds[i].clip != null)
            {
                validSounds.Add((motivationalSounds[i].clip, motivationalSounds[i].volume, i));
            }
        }

        if (validSounds.Count == 0)
        {
            return;
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, validSounds.Count);
        } while (validSounds[newIndex].index == lastMotivationalIndex && validSounds.Count > 1);

        var chosen = validSounds[newIndex];
        lastMotivationalIndex = chosen.index;

        audioSource.PlayOneShot(chosen.clip, chosen.volume);
    }
}
