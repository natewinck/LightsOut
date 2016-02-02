using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour {
  public AudioMixerSnapshot playerAudioOnly;
  public AudioMixerSnapshot fullVolume;

  [FormerlySerializedAs("PenaltiesBeforeLoss")]
  public int penaltiesBeforeLoss = 3;
  [System.NonSerialized]
  public int penaltyCount;

  [System.NonSerialized]
  public GameState gameState;

  private static GameManager _instance;
  public static GameManager instance {
    get { return _instance; }
  }

  void Awake() {
    GameManager._instance = this;
    gameState = new GameState();
  }

  void Start () {
    gameState.On("penalty", OnPenalty);
    gameState.On("nextlevel", OnNextLevel);
    gameState.On("replaylevel", OnReplayLevel);

    gameState.On("intro", _ => playerAudioOnly.TransitionTo(0.5f));
    gameState.On("win", _ => playerAudioOnly.TransitionTo(0.5f));
    gameState.On("lose", _ => playerAudioOnly.TransitionTo(0.5f));
    gameState.On("playing", _ => fullVolume.TransitionTo(0.5f));
  }

  void Update () {
    // Quit on Escape key.
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Debug.Log("Quit pressed!");
      Application.Quit();
    }
  }

  void OnPenalty(string oldState) {
    penaltyCount++;
    Debug.Log(penaltyCount + " of " + penaltiesBeforeLoss + " penalties");
    if (penaltyCount >= penaltiesBeforeLoss) {
      gameState.TransitionTo("lose");
    }
  }

  // Reload current scene.
  void OnReplayLevel(string oldState) {
    Debug.Log("Reloading level for retry.");
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  void OnNextLevel(string oldState) {
    var scene = SceneManager.GetActiveScene();
    Debug.Log("Scene " + scene.buildIndex + " of " + SceneManager.sceneCount + " finished. Next level!");
    if (scene.buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
      // Play "all done" sound? Credits?
      Debug.Log("No more levels! Quitting!");
      Application.Quit();
    } else {
      // Load the next scene per build order in Build Settings.
      SceneManager.LoadScene(scene.buildIndex + 1);
    }
  }
}
