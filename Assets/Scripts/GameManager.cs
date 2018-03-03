using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Play button
    [SerializeField]
    private Button playButton;

    // Step button
    [SerializeField]
    private Button stepButton;

    // 2D Game
    [SerializeField]
    private GameOfLife game2D;

    // 3D Game
    [SerializeField]
    private GameOfLife game3D;

    // The amount of time a single game tick takes in seconds
    [SerializeField]
    private Slider tickInterval;

    private GameOfLife game;
    private bool modeIs2d;

    private void Awake() {
        game = game2D;
        game.SetTickInterval(tickInterval.value);
    }

    public void SetTickInterval(float interval) {
        game.SetTickInterval(interval);
    }

    public void Pause() {
        game.Pause();
    }

    public void Step() {
        game.Tick();
    }

    public void ResetGame() {
        game.Restart();
    }

    public void ChangeGameMode() {
        modeIs2d = !modeIs2d;
    }

    public void Quit() {
        SceneManager.LoadScene("Main Menu");
    }
}
