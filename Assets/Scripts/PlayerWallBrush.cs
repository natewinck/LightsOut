using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// If player is moving and has their hand in a wall, bring volume up.

public class PlayerWallBrush : MonoBehaviour {
  [RangeAttribute(0.0f, 1.0f)]
  public float m_MaxVolume = 0.5f;

  private GameObject player;
  private GameObject m_CurrentWall;
  private AudioSource m_HandAudioSource; // Child object of this one
  private Vector3 m_LastPos;
  private bool m_IsMoving;

  private bool m_RunningRaiseCoroutine = false;
  private bool m_RunningLowerCoroutine = false;

  private int m_WallCount;

  private const float VOLUME_STEP = 0.05f;

  void Awake()
  {
    // Get the child object's audio source, which is on the hand
    m_HandAudioSource = GetComponentInChildren<AudioSource>();
    m_LastPos = transform.position;
    m_HandAudioSource.volume = 0.0f;
  }

  void Start() {
    player = GameObject.Find("Player");
  }

  void Update()
  {
    Vector3 currentPosition = player.transform.position;
    // Vector3 currentPosition = transform.position;
    // If this position is the same as the last position recorded...
    //Debug.Log("Difference: " + (currentPosition.z - m_LastPos.z).ToString());
    var wasMoving = m_IsMoving;
    m_IsMoving = currentPosition != m_LastPos;
    var startedMoving = !wasMoving && m_IsMoving;
    var stoppedMoving = wasMoving && !m_IsMoving;
    m_LastPos = currentPosition;

    // Stop here if not brushing or no change in movement.
    if (m_WallCount == 0 || !startedMoving && !stoppedMoving) return;

    StopAllCoroutines();

    if (startedMoving) {
Debug.Log("Raising volume on move, wallcount: " + m_WallCount);
      StartCoroutine(RaiseVolume(0.5f));
    } else {
      StartCoroutine(LowerVolume(0.3f));
    }
  }

  void OnTriggerEnter(Collider coll) {
    // Debug.Log ("I'm collidering with " + coll.gameObject.name);
    if (coll.gameObject.CompareTag("Wall") && coll.gameObject.GetComponent<SoundBank>() != null)
    {
      Debug.Log ("Entered the collider");
      m_WallCount++;

      // So we don't keep changing the audio clips when we're on the same wall,
      // Store the object as a reference
      m_CurrentWall = coll.gameObject;

      // Get the audio source from the wall
      AudioClip clip = m_CurrentWall.GetComponent<SoundBank> ().Draw(SoundBank.WALLBRUSHES);

      // Get the child of this (which should be the hand) and add this audio clip to it, then play
      m_HandAudioSource.clip = clip;
      m_HandAudioSource.Play ();
Debug.Log("Raising volume on trigger enter");
      // StopAllCoroutines ();
      // StartCoroutine (RaiseVolume(0.25f));


    }
  }

  void OnTriggerExit(Collider coll) {
    // Debug.Log ("exiting " + coll.gameObject.name);
    // We're no longer in this collider so stop playing the sound
    if (coll.gameObject.CompareTag("Wall"))
    {
      Debug.Log ("exiting");
      m_WallCount--;

      if (m_WallCount == 0) {
        StopAllCoroutines ();
        StartCoroutine (LowerVolume (0.2f));
      }
    }
  }

  IEnumerator RaiseVolume(float length) {
   while (m_HandAudioSource.volume < m_MaxVolume) {
      m_RunningRaiseCoroutine = true;
      yield return new WaitForSeconds (length / (m_MaxVolume / VOLUME_STEP));
      m_HandAudioSource.volume += VOLUME_STEP;
    }
    m_RunningRaiseCoroutine = false;
  }

  IEnumerator LowerVolume(float length) {
    while (m_HandAudioSource.volume > 0.0f) {
      m_RunningLowerCoroutine = true;
      yield return new WaitForSeconds (length / (m_MaxVolume / VOLUME_STEP));
      m_HandAudioSource.volume -= VOLUME_STEP;
    }
    m_RunningLowerCoroutine = false;
  }
}

