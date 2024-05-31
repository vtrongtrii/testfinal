using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    public AudioClip musicClip;
    public AudioClip strikePlayerClip;
    public AudioClip musicDie;
    public AudioClip musicClickButton;

    public float attackPitch = 1.5f; // Tốc độ phát cho âm thanh chém
    // Start is called before the first frame update
    void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }
    public void PlaySFX(AudioClip sfxClip)
    {
        vfxAudioSource.clip = sfxClip;

        // Đặt tốc độ phát cho âm thanh chém
        vfxAudioSource.pitch = attackPitch;

        vfxAudioSource.PlayOneShot(sfxClip);
    }
}
