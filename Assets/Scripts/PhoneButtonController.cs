using UnityEngine;
using System.Collections;

public class PhoneButtonController : MonoBehaviour {

  public UnityStandardAssets.Characters.FirstPerson.TankPersonController tankPersonController;
  public float resetTimeout = 4f;
  private float origResetTimeout;
  private GameState gameState;

#if UNITY_ANDROID

  void Awake() {
    origResetTimeout = resetTimeout;
    gameState = GameManager.instance.gameState;
  }

  void Update () {
    // Touch to walk forward.
    if (Input.touchCount > 0) {
      tankPersonController.PushForward();
    } else {
      tankPersonController.StopPushForward();
    }

    // Touch with 4+ fingers for `resetTimeout` seconds to restart the game.
    if (Input.touchCount >= 4 && resetTimeout > 0) {
      resetTimeout -= Time.deltaTime;
      if (resetTimeout <= 0) {
        gameState.TransitionTo("restartgame");
      }
    } else {
      resetTimeout = origResetTimeout;
    }
  }

#endif
}
