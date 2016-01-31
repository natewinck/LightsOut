using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMutter : MonoBehaviour {
  public AudioSource m_MouthAudioSource;

  private GameObject m_CurrentWall;
  private Vector3 m_LastPos;
  private bool m_IsMoving;

  void Awake()
  {
    m_LastPos = transform.position;
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
      m_MouthAudioSource.clip = clip;
      m_MouthAudioSource.Play ();

      // Replace the clip in the first person controller for feet
      //GetComponent<UnityStandardAssets.Characters.FirstPerson.TankPersonController>().ChangeFootstepSounds(clips);

      // That's it
    }
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
      if (m_MouthAudioSource.isPlaying)
      {
        m_MouthAudioSource.Stop ();
      }
    }
    else if (m_IsMoving && collision.gameObject.CompareTag("Wall")) // if velocity is not zero
    {
      Debug.Log ("hitting");
      if (!m_MouthAudioSource.isPlaying)
      {
        m_MouthAudioSource.Play ();
      }
    }
  }

  void OnCollisionExit(Collision collision)
  {
    //Debug.Log ("exiting");
    // We're no longer in this collider so stop playing the sound
    if (m_MouthAudioSource.isPlaying && collision.gameObject.CompareTag("Wall"))
    {
      Debug.Log ("exiting");
      m_MouthAudioSource.Stop ();
    }
  }
}
