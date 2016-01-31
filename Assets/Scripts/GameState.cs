using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// YAY STATE MACHINES
// gameState.On("start", (oldState) => { Debug.Log("Game Starting from " + oldState"); });
// gameState.OnExit("start", (newState) => { Debug.Log("Not starting anymore, now " + newState); });
// gameState.OnTransition("start", "play", (newState) => { Debug.Log("start => play"); });
// gameState.on("gameover", OnGameOver);
// void OnGameOver(newState) { Debug.Log("Game over man!"); }
class GameState {
  private static GameState _instance;
  public static GameState instance {
    get { return _instance == null ? _instance = new GameState() : _instance; }
  }

  string currentState;

  private Dictionary<string, List<System.Action<string>>> handlers;

  public GameState() {
    handlers = new Dictionary<string, List<System.Action<string>>>();
    currentState = "init";
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

  public IEnumerator DelayedTransitionTo(string newState, float delay) {
    yield return new WaitForSeconds(delay);
    TransitionTo(newState);
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
