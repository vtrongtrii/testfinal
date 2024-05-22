using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public static AudioSource buttonAudioSource; // Biến static để tất cả các instance của lớp này chia sẻ cùng một đối tượng AudioSource
    public AudioClip buttonClickSound; // Âm thanh của nút
    public float playbackSpeed = 2.0f; // Giá trị tốc độ playback
    void Awake()
    {
        // Kiểm tra xem đã có đối tượng AudioSource chưa, nếu chưa thì tạo mới
        if (buttonAudioSource == null)
        {
            GameObject audioSourceObj = new GameObject("ButtonAudioSource");
            buttonAudioSource = audioSourceObj.AddComponent<AudioSource>();
            // Gán âm thanh cho AudioSource từ giao diện người dùng hoặc thông qua mã nếu cần
            buttonAudioSource.clip = buttonClickSound;
        }
    }

    public void PlayButtonClickSound()
    {
        if (buttonAudioSource != null && buttonAudioSource.clip != null)
        {
            buttonAudioSource.pitch = playbackSpeed; // Đặt tốc độ playback
            buttonAudioSource.Play();
        }
    }
}