using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    //audio clips
    public AudioClip background;
    public AudioClip click;
    public AudioClip toggle;
    public AudioClip correct;
    public AudioClip wrong;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void MuteMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = true;
        }
    }

    public void UnmuteMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = false;
        }
    }

    public void GraduallySpeedUpMusic(float targetSpeed, float duration)
    {
        StartCoroutine(AdjustMusicSpeed(targetSpeed, duration));
    }

    public void GraduallySlowDownMusic(float targetSpeed, float duration)
    {
        StartCoroutine(AdjustMusicSpeed(targetSpeed, duration));
    }

    private IEnumerator AdjustMusicSpeed(float targetSpeed, float duration)
    {
        if (musicSource == null) yield break;

        float startSpeed = musicSource.pitch;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.pitch = Mathf.Lerp(startSpeed, targetSpeed, elapsed / duration);
            yield return null;
        }

        musicSource.pitch = targetSpeed;
    }
}
