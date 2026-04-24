using UnityEngine;

public class AudioDemoManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource soundEffectSource;

    [Header("Music Clips")]
    public AudioClip backgroundMusic;
    public AudioClip levelClearMusic;
    public AudioClip deathMusic;

    [Header("Sound Effect Clips")]
    public AudioClip playerAttackSound;
    public AudioClip enemyAttackSound;

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayPlayerAttackSound()
    {
        soundEffectSource.PlayOneShot(playerAttackSound);
    }

    public void PlayEnemyAttackSound()
    {
        soundEffectSource.PlayOneShot(enemyAttackSound);
    }

    public void PlayLevelClearMusic()
    {
        backgroundMusicSource.clip = levelClearMusic;
        backgroundMusicSource.loop = false;
        backgroundMusicSource.Play();
    }

    public void PlayDeathMusic()
    {
        backgroundMusicSource.clip = deathMusic;
        backgroundMusicSource.loop = false;
        backgroundMusicSource.Play();
    }
}