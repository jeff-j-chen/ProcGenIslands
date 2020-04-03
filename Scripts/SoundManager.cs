using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] AudioClip[] audioClips;
    AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(string clipName) {
        audioSource.PlayOneShot(audioClips[(from clip in audioClips select clip.name).ToList().IndexOf(clipName)]);
    }
}
