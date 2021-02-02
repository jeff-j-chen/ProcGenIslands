using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public AudioClip[] songs;
    // array of songs to play
    AudioSource audioSource;
    // audiosource attached to this
    void Start() {
        audioSource = GetComponent<AudioSource>();
        // get the audiosource
        SetUpSingleton();
        // ensure the music player persists through scenes
        StartCoroutine(PlaySongs());
        // begin playing songs
    }
    
    private IEnumerator PlaySongs() {
        yield return new WaitForSeconds(30);
        // wait 30s before starting the first song
        while (true) {
            // repeat forever
            audioSource.PlayOneShot(songs[Random.Range(0, songs.Length)]);
            // play a random song
            yield return new WaitForSeconds(Random.Range(300, 601));
            // wait 5-10 minutes before playing another
        }
    }

    private void SetUpSingleton() {
        // used to make a gameobject persist through scenes
        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
}
