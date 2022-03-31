using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip vanishClip;
    [SerializeField] [Range(0f, 1f)] float vanishVolume = 0.5f;

    static AudioPlayer Instance;

    private void Awake()
    {
        ManageSingleton();
    }

    private void ManageSingleton()
    {
        if (Instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayVanishClip()
    {
        PlayClip(vanishClip, vanishVolume);
    }

    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}
