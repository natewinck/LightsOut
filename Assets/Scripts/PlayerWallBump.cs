using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWallBump : MonoBehaviour {

  private GameObject m_CurrentWall;
  private AudioSource m_HandAudioSource; // Child object of this one
  private Vector3 m_LastPos;
  private bool m_IsMoving;
  private float m_Velocity;

  void Awake()
  {
    // Get the child object's audio source, which is on the hand
    m_HandAudioSource = GetComponentInChildren<AudioSource>();
    m_LastPos = transform.position;
  }

  void Update()
  {
    Vector3 currentPosition = transform.position;
    // If this position is the same as the last position recorded...
    if (currentPosition == m_LastPos) {
      m_IsMoving = false;
      m_Velocity = 0.0f;
    } else {
      // Calculate the velocity
      m_Velocity = (currentPosition - m_LastPos).magnitude / Time.deltaTime;

      m_IsMoving = true;
      // Update the last position
      m_LastPos = currentPosition;
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag ("Wall")) {
      Debug.Log ("You hit the wall with force " + m_Velocity);
    }
    /*Debug.Log ("I'm collisioning with " + collision.gameObject.name);
    if (collision.gameObject.CompareTag("Wall") && collision.gameObject.GetComponent<SoundBank>() != null)
    {
      // So we don't keep changing the audio clips when we're on the same floor,
      // Store the object as a reference
      m_CurrentWall = collision.gameObject;

      // Get the audio source from the wall
      AudioClip clip = m_CurrentWall.GetComponent<SoundBank> ().Draw();

      // Get the child of this (which should be the hand) and add this audio clip to it, then play
      m_HandAudioSource.clip = clip;
      m_HandAudioSource.Play ();

      // Replace the clip in the first person controller for feet
      //GetComponent<UnityStandardAssets.Characters.FirstPerson.TankPersonController>().ChangeFootstepSounds(clips);

      // That's it
    }
    */
  }

  void OnCollisionStay(Collision collision)
  {
    // Vector3 velocity = collision.relativeVelocity;
    // The above does not work because the Player is moving, not the hands (though they are in absolute space)
    // Only play audio when the velocity when we're moving
    //Debug.Log(velocity.x + ", " + velocity.y + ", " + velocity.z);
    //Debug.Log(velocity.magnitude);
    /*if (!m_IsMoving)
    {
      Debug.Log ("I'm moving");
      if (m_HandAudioSource.isPlaying)
      {
        m_HandAudioSource.Stop ();
      }
    }
    else // if velocity is not zero
    {
      if (!m_HandAudioSource.isPlaying)
      {
        m_HandAudioSource.Play ();
      }
    }*/
  }

  void OnCollisionExit(Collision collision)
  {
    //Debug.Log ("exiting");
    // We're no longer in this collider so stop playing the sound
    /*if (m_HandAudioSource.isPlaying)
    {
      m_HandAudioSource.Stop ();
    }*/
  }
}
