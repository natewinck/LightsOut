using UnityEngine;
using System.Collections;

public class PhoneOrientation : MonoBehaviour {

  //Gyro
  public float smooth = 2.0F;
  public float tiltAngle = 90.0F;

  private Quaternion initialRotation;
  private Quaternion gyroInitialRotation;

	// Use this for initialization
	void Start () {
    Input.gyro.enabled = true;

    initialRotation = transform.rotation; 
    gyroInitialRotation = Input.gyro.attitude;

    //Debug.Log ("INITIAL: " + Input.gyro.attitude.eulerAngles.ToString());

	}

  public void ResetOrientation() {
    gyroInitialRotation = Input.gyro.attitude; 
    //Debug.Log ("reset");
  }
	
	// Update is called once per frame
	void Update () {
    //Gyro
    //float tiltAroundZ = -Input.acceleration.x * tiltAngle * 50;
    //float tiltAroundX = Input.acceleration.y * tiltAngle * 50;
    //Quaternion target = Quaternion.Euler(0, tiltAroundZ, 0);
    //Debug.Log (Input.compass.rawVector.x + ", " + Input.compass.rawVector.y + ", " + Input.compass.rawVector.z);

    //transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);

    //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    //transform.rotation = new Quaternion( Input.acceleration.z;
    //Debug.Log (Input.acceleration.x + ", " + Input.acceleration.y + ", " + Input.acceleration.z);

    //SystemInfo.supportsGyroscope;
    /*Input.gyro.enabled = true;
    Debug.Log(Input.gyro.attitude.ToString ());
    //Debug.Log (Input.compass.rawVector.ToString());

    float angle;
    Vector3 axis;

    Vector3 eulerAngle = Input.gyro.attitude.eulerAngles;
    Input.gyro.attitude
    Debug.Log (eulerAngle.ToString());

    // Restrict to just Y Axis
    //Vector3 YAxisRotation = Vector3.up * axis;

    transform.rotation = Quaternion.Euler (eulerAngle.x, eulerAngle.y, eulerAngle.z);*/



    //Debug.Log (Input.gyro.attitude.eulerAngles.ToString());
    Quaternion offsetRotation = Quaternion.Inverse(gyroInitialRotation) * Input.gyro.attitude;
    //transform.rotation = initialRotation * offsetRotation;

    // me: to lock on y axis
    Quaternion yourRotationQuaternion = Quaternion.Slerp(initialRotation * offsetRotation, transform.rotation, 0.1f);
    transform.rotation = Quaternion.Inverse( Quaternion.Euler(new Vector3(yourRotationQuaternion.eulerAngles.x, yourRotationQuaternion.eulerAngles.y, -yourRotationQuaternion.eulerAngles.z)) );

    // The other guy's example:
    //transform.rotation = ConvertRotation(Input.gyro.attitude) * GetRotFix();

    //Quaternion.Slerp(

    //Debug.Log (tiltAroundX + ", " + tiltAroundZ);
	}
}
