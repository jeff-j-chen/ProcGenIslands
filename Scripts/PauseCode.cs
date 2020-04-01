using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCode : MonoBehaviour {

    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    void Start() {
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
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
}
