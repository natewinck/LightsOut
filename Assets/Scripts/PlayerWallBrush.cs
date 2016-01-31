using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWallBrush : MonoBehaviour {

  private GameObject m_CurrentWall;
  private AudioSource m_HandAudioSource; // Child object of this one
  private Vector3 m_LastPos;
  private bool m_IsMoving;

  private bool m_RunningRaiseCoroutine = false;
  private bool m_RunningLowerCoroutine = false;

  private IEnumerator coroutine;

  void Awake()
  {
    // Get the child object's audio source, which is on the hand
    m_HandAudioSource = GetComponentInChildren<AudioSource>();
    m_LastPos = transform.position;
    m_HandAudioSource.volume = 0.0f;
  }

  void Update()
  {
    Vector3 currentPosition = transform.position;
    // If this position is the same as the last position recorded...
    //Debug.Log("Difference: " + (currentPosition.z - m_LastPos.z).ToString());
    if (currentPosition == m_LastPos) {
      m_IsMoving = false;
    } else {
      m_IsMoving = true;
      // Update the last position
      m_LastPos = currentPosition;
    }
  }

  // Used with the Character Controller
  void OnControllerColliderHit(ControllerColliderHit hit)
  {

    //Debug.Log ("Hello");
    /*if (hit.gameObject.CompareTag("Wall") && hit.gameObject.GetComponent<SoundBank>() != null)
    {
      // So we don't keep changing the audio clips when we're on the same floor,
      // Store the object as a reference
      m_CurrentWall = hit.gameObject;

      // Get the audio source from the wall
      AudioClip clip = hit.gameObject.GetComponent<SoundBank> ().Draw();

      // Replace the clip in the first person controller for feet
      GetComponent<UnityStandardAssets.Characters.FirstPerson.TankPersonController>().ChangeFootstepSounds(clips);

      // That's it
    }*/
  }

  void OnCollisionEnter(Collision collision)
  {
    //Debug.Log ("I'm collisioning with " + collision.gameObject.name);
    if (collision.gameObject.CompareTag("Wall") && collision.gameObject.GetComponent<SoundBank>() != null)
    {
      // So we don't keep changing the audio clips when we're on the same floor,
      // Store the object as a reference
      m_CurrentWall = collision.gameObject;

      // Get the audio source from the wall
      AudioClip clip = m_CurrentWall.GetComponent<SoundBank> ().Draw(SoundBank.WALLBRUSHES);

      // Get the child of this (which should be the hand) and add this audio clip to it, then play
      m_HandAudioSource.clip = clip;
      m_HandAudioSource.Play ();

      StopAllCoroutines ();
      StartCoroutine (RaiseVolume(0.5f));

      Debug.Log ("Entered the collider");
      /*
      if (m_RunningRaiseCoroutine) {
        // Do nothing
      } else if (m_RunningLowerCoroutine) {
        // Stop it, then raise it
        StopCoroutine(coroutine);
        coroutine = RaiseVolume (0.5f);
        StartCoroutine (coroutine);
      } else { // If nothing is running...
        coroutine = RaiseVolume (0.5f);
        StartCoroutine (coroutine);
      }
      */



      // Replace the clip in the first person controller for feet
      //GetComponent<UnityStandardAssets.Characters.FirstPerson.TankPersonController>().ChangeFootstepSounds(clips);

      // That's it
    }
  }

  IEnumerator RaiseVolume(float length) {
    while (m_HandAudioSource.volume < 1.0f) {
      m_RunningRaiseCoroutine = true;
      yield return new WaitForSeconds (length / 20.0f);
      m_HandAudioSource.volume += .05f;
    }
    m_RunningRaiseCoroutine = false;
  }

  IEnumerator LowerVolume(float length) {
    while (m_HandAudioSource.volume > 0.0f) {
      m_RunningLowerCoroutine = true;
      yield return new WaitForSeconds (length / 20.0f);
      m_HandAudioSource.volume -= .05f;
    }
    m_RunningLowerCoroutine = false;
  }

  void OnCollisionStay(Collision collision)
  {
    // Vector3 velocity = collision.relativeVelocity;
    // The above does not work because the Player is moving, not the hands (though they are in absolute space)
    // Only play audio when the velocity when we're moving
    //Debug.Log(velocity.x + ", " + velocity.y + ", " + velocity.z);
    //Debug.Log(velocity.magnitude);
    if (!m_IsMoving)
    {
      Debug.Log ("No longer moving but in it");
      StopAllCoroutines ();
      StartCoroutine(LowerVolume(0.2f));
      /*
      if (m_RunningLowerCoroutine) {
        // Do nothing. Keep running that coroutine!
      } else if (m_RunningRaiseCoroutine) {
        // Stop that and lower it!
        StopCoroutine (coroutine);
        coroutine = LowerVolume (0.2f);
        StartCoroutine (coroutine);
        //m_HandAudioSource.Stop ();
      } else { // Just in case the volume's all the way up, run lower
        coroutine = LowerVolume (0.2f);
        StartCoroutine (coroutine);
      }*/
    }
    else if (m_IsMoving && collision.gameObject.CompareTag("Wall")) // if velocity is not zero
    {
      //Debug.Log ("moving inside");
      // Need to check to see if the coroutine is already running

      StopAllCoroutines ();
      StartCoroutine(RaiseVolume(0.1f));
      /*
      if (m_RunningRaiseCoroutine) {
        // Do nothing
        Debug.Log("doing nothing");
      } else if (m_RunningLowerCoroutine) {
        // Stop that coroutine and raise it
        StopCoroutine (coroutine);
        coroutine = RaiseVolume (0.1f);
        StartCoroutine (coroutine);
        //m_HandAudioSource.Play ()
        Debug.Log("Stop and raise");
      } else { // If no coroutine is running raise the volume just in case it was just lowered all the way
        coroutine = RaiseVolume (0.1f);
        StartCoroutine (coroutine);
        Debug.Log ("Raising the volume");
      }
      */
    }
  }

  void OnCollisionExit(Collision collision)
  {
    //Debug.Log ("exiting " + collision.gameObject.name);
    // We're no longer in this collider so stop playing the sound
    if (collision.gameObject.CompareTag("Wall"))
    {
      Debug.Log ("exiting");

      StopAllCoroutines ();
      StartCoroutine (LowerVolume (0.3f));
      /*
      if (m_RunningLowerCoroutine) {
        // Do nothing
      } else { // If nothing is running or it's being raised...
        StopCoroutine (coroutine);
        coroutine = LowerVolume (0.3f);
        StartCoroutine (coroutine);
        //m_HandAudioSource.Stop ();
      }
      */
     
    }
  }
}
