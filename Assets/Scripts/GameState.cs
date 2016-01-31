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
/* States:
- init (default)
- intro
- playing
- win
- lose
- replaylevel
- nextlevel
- quit
*/

public class GameState {
  private static GameState _instance;
  public static GameState instance {
    get { return _instance == null ? _instance = new GameState() : _instance; }
  }

  string currentState;

  private Dictionary<string, List<System.Action<string>>> handlers;
  private bool isWaitingForTransition;

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
    if (currentHandlers == null) currentHandlers = new List<System.Action<string>>();
    currentHandlers.Add(handler);
    handlers[stateOrTransition] = currentHandlers;
  }

  public string GetState() {
    return currentState;
  }

  // Delay a state machine transition. Designed to transition when a clip
  // finishes playing.
  // NOTE: An immediate transition will cancel all pending delayed transitions.
  //   And a delayed transition firing will cancel all other pending delayed
  //   transitions.
  public IEnumerator DelayedTransitionTo(string newState, float delay) {
    if (isWaitingForTransition) {
      Debug.Log("*** WARNING! DelayedTransitionTo called, but another delayed transition is already queued! WEIRDNESS AHEAD");
    }

    isWaitingForTransition = true;

    yield return new WaitForSeconds(delay);

    // Still waiting? Transition.
    if (isWaitingForTransition) {
      TransitionTo(newState);
    }
  }

  public void TransitionTo(string newState) {
    var oldState = currentState;
    var enterState = newState;
    var transition = currentState + " => " + newState;
    var exitState = "exit-" + currentState;
    var transitions = new string[] {enterState, transition, exitState};
    List<System.Action<string>> someCallbacks;
Debug.Log("STATE: " + transition);
    currentState = newState;
    isWaitingForTransition = false;

    // Look up exit handlers & fire callbacks.
    someCallbacks = _getCallbacks(exitState);
    foreach (var callback in someCallbacks) { callback(newState); }

    // Look up transition handlers & fire callbacks.
    someCallbacks = _getCallbacks(transition);
    foreach (var callback in someCallbacks) { callback(newState); }

    // Look up enter handlers & fire callbacks.
    someCallbacks = _getCallbacks(enterState);
    foreach (var callback in someCallbacks) { callback(oldState); }

  }

  private List<System.Action<string>> _getCallbacks(string state) {
    List<System.Action<string>> someCallbacks;

    handlers.TryGetValue(state, out someCallbacks);
    if (someCallbacks == null) someCallbacks = new List<System.Action<string>>();

    return someCallbacks;
  }
}
