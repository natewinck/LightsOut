using UnityEngine;
using System.Collections;

public class PhoneButtonController : MonoBehaviour {

  public UnityStandardAssets.Characters.FirstPerson.TankPersonController tankPersonController;

  public LayerMask normalLayers;
  public LayerMask revealLayers;

  [System.NonSerialized]
  private float revealTimeout = 3f;
  private float origRevealTimeout;
  private float resetTimeout = 3f;
  private float origResetTimeout;
  private GameState gameState;

#if UNITY_ANDROID

  void Awake() {
    origResetTimeout = resetTimeout;
    origRevealTimeout = revealTimeout;
    gameState = GameManager.instance.gameState;
  }

  void Update () {
    // Touch to walk forward.
    if (Input.touchCount > 0) {
      tankPersonController.PushForward();
    } else {
      tankPersonController.StopPushForward();
    }

    // Touch with 3 fingers for `resetTimeout` seconds to restart the game.
    if (Input.touchCount == 3 && resetTimeout > 0) {
      resetTimeout -= Time.deltaTime;
      if (resetTimeout <= 0) {
        gameState.TransitionTo("restartgame");
      }
    } else {
      resetTimeout = origResetTimeout;
    }

    // Touch with 4+ fingers for `revealTimeout` seconds to turn on the camera.
    if (Input.touchCount >= 4 && revealTimeout > 0) {
      revealTimeout -= Time.deltaTime;
      if (revealTimeout <= 0) {
        if (Camera.main.cullingMask == normalLayers) {
          Camera.main.cullingMask = revealLayers;
        } else {
          Camera.main.cullingMask = normalLayers;
        }
      }
    } else {
      revealTimeout = origResetTimeout;
    }

  }

  // void OnGUI () {
  //     GUI.Label(new Rect (10, 10, 200, 20), Input.touchCount.ToString() );
  // }

#endif


}
