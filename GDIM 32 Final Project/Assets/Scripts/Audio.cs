using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.OnHealthChanged += PlayAudio;
    }

    private void PlayAudio(int health)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void OnDestory()
    {
        GameEvents.OnHealthChanged -= PlayAudio;
    }
}
