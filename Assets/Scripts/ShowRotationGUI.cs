using UnityEngine;
using System.Collections;

public class ShowRotationGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

  void OnGUI () {
      GUI.contentColor = Color.grey;
      GUI.Label(new Rect (Screen.width - 35, Screen.height - 30, 200, 20), Mathf.RoundToInt(transform.rotation.eulerAngles.y).ToString() );
  }

}
