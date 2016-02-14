using UnityEngine;

public class CompassCam : MonoBehaviour
{
    private bool compassEnabled;
    private Compass compass;

    private const float SmoothFactorCompass = 0.25f;
    private const float SmoothThresholdCompass = 30.0f;
    private float oldHeading;
    private float compassSnoozeTime = 0.25f;
    private float originalHeading;
    private Quaternion counterInitialRotation = Quaternion.identity;

    public void Awake() {
        #if UNITY_ANDROID
        compassEnabled = true;
        #else
        compassEnabled = false;
        #endif

        if (compassEnabled) {
            compass = Input.compass;
            compass.enabled = true;
        } else {
            #if UNITY_EDITOR
            Debug.Log("NO COMPASS");
            #endif
        }

        originalHeading = transform.rotation.eulerAngles.y;
    }

    public void Update ()
    {
        if (compassEnabled) {
            // Compass returns 0 at start of game. Gotta wait for it to warm up before initial reading?
            if (compassSnoozeTime > 0) {
                compassSnoozeTime -= Time.deltaTime;
                if (compassSnoozeTime <= 0) ResetOrientation();
            }

            var newHeading = compass.magneticHeading;
            oldHeading = DampIt(oldHeading, newHeading);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, oldHeading, 0) * counterInitialRotation, Time.deltaTime * 8);
        }
    }

    public void ResetOrientation() {
        if (compassEnabled) {
            var heading = compass.magneticHeading;
            counterInitialRotation = Quaternion.Euler(0, originalHeading - heading, 0);
        }
    }

    // http://stackoverflow.com/questions/4699417/android-compass-orientation-on-unreliable-low-pass-filter#6462517
    private float DampIt(float oldCompass, float newCompass) {
        if (Mathf.Abs(newCompass - oldCompass) < 180) {
            if (Mathf.Abs(newCompass - oldCompass) > SmoothThresholdCompass) {
                oldCompass = newCompass;
            }
            else {
                oldCompass = oldCompass + SmoothFactorCompass * (newCompass - oldCompass);
            }
        }
        else {
            if (360.0 - Mathf.Abs(newCompass - oldCompass) > SmoothThresholdCompass) {
                oldCompass = newCompass;
            }
            else {
                if (oldCompass > newCompass) {
                    oldCompass = (oldCompass + SmoothFactorCompass * ((360 + newCompass - oldCompass) % 360) + 360) % 360;
                }
                else {
                    oldCompass = (oldCompass - SmoothFactorCompass * ((360 - newCompass + oldCompass) % 360) + 360) % 360;
                }
            }
        }

        return oldCompass;
    }

    // void OnGUI() {
    //     var st = new GUIStyle();
    //     st.fontSize = 48;
    //     GUILayout.Label("raw compass heading: " + Input.compass.magneticHeading, st);
    //     GUILayout.Label("oldHeading: " + oldHeading, st);
    //     GUILayout.Label("originalHeading: " + originalHeading, st);
    //  }

}

