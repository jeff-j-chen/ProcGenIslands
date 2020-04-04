using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public AudioClip[] audioClips;
    // array of sound clips
    private AudioSource audioSource;
    // audiosource attached to gameobject
    void Start() {
        audioSource = GetComponent<AudioSource>();
        // get the audiosource
    }

    public void PlayClip(string clipName) {
        audioSource.PlayOneShot(audioClips[(from clip in audioClips select clip.name).ToList().IndexOf(clipName)]);
        // play a clip with given clip name (e.g. "walking0") by playing the clip at the index of which the clip's name occurred
    }
}
