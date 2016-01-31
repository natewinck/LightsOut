using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {
  public int PenaltiesBeforeLoss = 3;

  private GameState gameState;
  private int penaltyCount;

  void Start () {
    gameState = GameState.instance;
    gameState.On("penalty", OnPenalty);
    gameState.On("nextlevel", OnNextLevel);
    gameState.On("replaylevel", OnReplayLevel);
  }

  void Update () {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Debug.Log("Quit pressed!");
      Application.Quit();
    }
  }

  void OnPenalty(string oldState) {
    penaltyCount++;
    Debug.Log(penaltyCount + " of " + PenaltiesBeforeLoss + " penalties");
    if (penaltyCount >= PenaltiesBeforeLoss) {
      gameState.TransitionTo("lose");
    }
  }

  // Reload current scene.
  void OnReplayLevel(string oldState) {
    Debug.Log("Reloading level for retry.");
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  void OnNextLevel(string oldState) {
    Debug.Log("Next level!");
    var scene = SceneManager.GetActiveScene();
    if (scene.buildIndex == SceneManager.sceneCount - 1) {
      // Play "all done" sound? Credits?
      Debug.Log("No more levels! Quitting!");
      Application.Quit();
    } else {
      // Load the next scene per build order in Build Settings.
      SceneManager.LoadScene(scene.buildIndex + 1);
    }
  }
}
