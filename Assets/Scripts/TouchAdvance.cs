using UnityEngine;
using System.Collections;

public class TouchAdvance : MonoBehaviour {
  public string onTouchSetState;

  private GameState gameState;
  private bool isReset;
  private bool isTransitioned;

  // Use this for initialization
  void Start () {
    gameState = GameManager.instance.gameState;
  }

  // Update is called once per frame
  void Update () {
    if (isReset && !isTransitioned && Input.touchCount == 1) {
      gameState.TransitionTo(onTouchSetState);
      isTransitioned = true;
    }

    isReset = isReset || Input.touchCount == 0;
  }
}
