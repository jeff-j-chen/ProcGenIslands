using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public AudioClip[] songs;
    AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        SetUpSingleton();
        StartCoroutine(PlaySongs());
    }

    private void SetUpSingleton() {
        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySong(int songNum) {
        audioSource.PlayOneShot(songs[songNum]);
    }
    
    private IEnumerator PlaySongs() {
        yield return new WaitForSeconds(30);
        while (true) {
            PlaySong(Random.Range(0, 3));
            yield return new WaitForSeconds(Random.Range(300, 601));
        }
    }
}
