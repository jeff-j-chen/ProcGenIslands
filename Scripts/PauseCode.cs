using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCode : MonoBehaviour {

    public bool gameIsPaused = false;
    // whether or not the game is paused
    public GameObject pauseMenuUI;
    // the gameobject with all the pause ui on it

    void Start() {
        pauseMenuUI.SetActive(false);
        // set the pause menu to be inactive (unseen)
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            // when player presses escape
            if (gameIsPaused) {
                AudioListener.volume = 1;
                Resume();
            }
            else {
                AudioListener.volume = 0;
                Pause();
            }
            // toggle the state (paused/unpaused) and change the listener volume accordingly
        }
    }
    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void LoadMenu() {
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame() {
        Application.Quit();
    }
    // pretty self explanatory, keep these functions out here as they are needed for hooking up to buttons
}
