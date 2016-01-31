using UnityEngine;
using System.Collections;

public class PhoneButtonController : MonoBehaviour {

  public UnityStandardAssets.Characters.FirstPerson.TankPersonController tankPersonController;
  public PhoneOrientation phoneOrientation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.touchCount > 0) {
      if(Input.GetTouch (0).position.x / Screen.width > 0.9f) {
        // Get movement of the finger since last frame
        //Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        phoneOrientation.ResetOrientation();
        // Move object across XY plane
        //transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0)
      } else {
        tankPersonController.PushForward();
      }
    } else {
      tankPersonController.StopPushForward();
    }
	}

  void OnPointerDown() {
    Debug.Log ("clicked");
  }

  void OnPointerUp() {
    Debug.Log ("Up");

  }
}
