using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
init (default)
l









*/


public class GameManager : MonoBehaviour {
  private static GameManager _instance;

  private int mistakeCount;
  private int maxMistakeCount;
  private StateMachine stateMachine;

  public static GameManager instance {
    get { return _instance ? _instance : Init(); }
  }

  private static GameManager Init() {
    _instance = new GameManager();
    return _instance;
  }

  // El Constructador
  public GameManager() {
    stateMachine = new StateMachine();
    stateMachine.On("gameover", OnGameOver);
    // stateMachine.On("")
  }

  public void AddMistake() {
    mistakeCount++;
    if (mistakeCount > maxMistakeCount) {
      stateMachine.TransitionTo("gameover");
    }
  }

  public void OnGameOver(string oldState) {
    Debug.Log("Game over, man!");
  }

  // YAY STATE MACHINES
  // stateMachine.On("start", (oldState) => { Debug.Log("Game Starting from " + oldState"); });
  // stateMachine.OnExit("start", (newState) => { Debug.Log("Not starting anymore, now " + newState); });
  // stateMachine.OnTransition("start", "play", (newState) => { Debug.Log("start => play"); });
  // stateMachine.on("gameover", OnGameOver);
  // void OnGameOver(newState) { Debug.Log("Game over man!"); }
  class StateMachine {
    string currentState;

    private Dictionary<string, List<System.Action<string>>> handlers;

    public StateMachine() {
      handlers = new Dictionary<string, List<System.Action<string>>>();
    }

    public void OnEnter(string state, System.Action<string> handler) {
      On(state, handler);
    }

    public void OnTransition(string fromState, string toState, System.Action<string> handler) {
      On(fromState + " => " + toState, handler);
    }

    public void OnExit(string state, System.Action<string> handler) {
      On("exit-" + state, handler);
    }

    public void On(string stateOrTransition, System.Action<string> handler) {
      // Look up current handler list.
      List<System.Action<string>> currentHandlers;
      handlers.TryGetValue(stateOrTransition, out currentHandlers);

      // Add new handler.
      currentHandlers.Add(handler);
      handlers[stateOrTransition] = currentHandlers;
    }

    public void TransitionTo(string newState) {
      var oldState = currentState;
      var enterState = newState;
      var transition = currentState + " => " + newState;
      var exitState = "exit-" + currentState;
      var transitions = new string[] {enterState, transition, exitState};
      List<System.Action<string>> someCallbacks;

      currentState = newState;

      // Look up exit handlers & fire callbacks.
      handlers.TryGetValue(exitState, out someCallbacks);
      foreach (var callback in someCallbacks) { callback(newState); }

      // Look up transition handlers & fire callbacks.
      handlers.TryGetValue(transition, out someCallbacks);
      foreach (var callback in someCallbacks) { callback(newState); }

      // Look up enter handlers & fire callbacks.
      handlers.TryGetValue(enterState, out someCallbacks);
      foreach (var callback in someCallbacks) { callback(oldState); }

    }
  }
}
