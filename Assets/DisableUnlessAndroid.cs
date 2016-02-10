using UnityEngine;

public class DisableUnlessAndroid : MonoBehaviour {

  void Awake() {
#if UNITY_ANDROID
#else
    gameObject.SetActive(false);
#endif
  }
}
