using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
init (default)
intro
playing
win
lose
replaylevel
nextlevel
quit

Start():
- startbox moves to player? player in startbox? transitions to game state to "intro"
- player looks for "intros" clips to mutter from soundbank in startbox, "isIntroDone = true"
- when finished, player unlocks itself and transitions game state to "playing"

on collision:
- mutter looks for tag "Goal"
- if tag "Goal", transitions game state to "win", plays internal win audio, on finished, transitions to outro
- MistakeDetector increments mistakes on collision w/bad things
- at 3 mistakes, mistake Detector transitions to "lose"

on "win":
- locks player
- plays internal win audio
- when finished, transitions to nextlevel

on "lose":
- plays parent scolding audio?
- plays "lose" built-in on player
- when finished, transitions to replaylevel
*/

public class PlayerMutter : MonoBehaviour {
  public AudioSource m_AudioSource;

  private GameState m_StateMachine;
  private SoundBank m_SoundBank;
  private GameObject m_CurrentWall;
  private Vector3 m_LastPos;
  private bool m_IsMoving;

  void Awake()
  {
    m_LastPos = transform.position;
    m_SoundBank = GetComponent<SoundBank>();

  }

  void Start() {
    m_StateMachine = GameManager.instance.gameState;
    m_StateMachine.On("win", OnWin);
    m_StateMachine.On("lose", OnLose);
    m_StateMachine.On("intro", OnIntro);
    m_StateMachine.TransitionTo("intro");
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

  void OnTriggerEnter(Collider other) {
    Debug.Log (other.name);
    var otherSoundBank = other.GetComponent<SoundBank>();

    if (otherSoundBank != null && !m_AudioSource.isPlaying) {
      PlayMutter(otherSoundBank, other);
    }

    CheckWin(other);
    CheckPenalty(otherSoundBank, other);
  }

  void PlayMutter(SoundBank otherSoundBank, Collider other) {
    // Get a mutter if exists.
    var clip = otherSoundBank.Draw(SoundBank.MUTTERS);
    if (clip == null) return;

    // Get the child of this (which should be the hand) and add this audio clip to it, then play
    m_AudioSource.clip = clip;
    m_AudioSource.PlayDelayed(0.8f);
  }

  void CheckWin(Collider other) {
    if (!other.CompareTag("Goal")) return;
    m_StateMachine.TransitionTo("win");
  }

  void CheckPenalty(SoundBank otherSoundBank, Collider other) {
    if (!other.CompareTag("Penalty")) return;
    m_StateMachine.TransitionTo("penalty");
  }

  void OnIntro(string oldState) {
    // Get a mutter if exists.
    var introBucket = GameObject.Find("GameManager");
    if (introBucket == null) return;
    var clip = introBucket.GetComponent<SoundBank>().Draw(SoundBank.INTROS);

    if (clip == null) {
      m_StateMachine.TransitionTo("playing");
    } else {
      // Get the child of this (which should be the hand) and add this audio clip to it, then play
      m_AudioSource.clip = clip;
      m_AudioSource.PlayDelayed(0.8f);
      StartCoroutine(m_StateMachine.DelayedTransitionTo("playing", 0.8f + clip.length));
    }
  }

  void OnWin(string oldState) {
    // Get a mutter if exists.
    var clip = m_SoundBank.Draw(SoundBank.WINS);
    if (clip == null) {
      m_StateMachine.TransitionTo("nextlevel");
    } else {
      // Get the child of this (which should be the hand) and add this audio clip to it, then play
      m_AudioSource.clip = clip;
      m_AudioSource.PlayDelayed(0.8f);
      StartCoroutine(m_StateMachine.DelayedTransitionTo("nextlevel", 0.8f + clip.length));
    }
  }

  void OnLose(string oldState) {
    // Get a mutter if exists.
    var clip = m_SoundBank.Draw(SoundBank.LOSSES);
    if (clip == null) {
      m_StateMachine.TransitionTo("replaylevel");
    } else {
      // Get the child of this (which should be the hand) and add this audio clip to it, then play
      m_AudioSource.clip = clip;
      m_AudioSource.PlayDelayed(0.8f);
      StartCoroutine(m_StateMachine.DelayedTransitionTo("replaylevel", 0.8f + clip.length));
    }
  }



      /*
  void OnCollisionEnter(Collision collision)
  {
    //Debug.Log ("I'm collisioning with " + collision.gameObject.name);
    if (collision.gameObject.CompareTag("MutterBox") && collision.gameObject.GetComponent<SoundBank>() != null)
    {
      Debug.Log ("I'm collisioning with Mutter Box");
      // So we don't keep changing the audio clips when we're on the same floor,
      // Store the object as a reference
      m_CurrentWall = collision.gameObject;

      // Get the audio source from the wall
      AudioClip clip = m_CurrentWall.GetComponent<SoundBank> ().Draw(SoundBank.MUTTERS);

      // Get the child of this (which should be the hand) and add this audio clip to it, then play
      m_AudioSource.clip = clip;
      m_AudioSource.Play ();

      // Replace the clip in the first person controller for feet
      //GetComponent<UnityStandardAssets.Characters.FirstPerson.TankPersonController>().ChangeFootstepSounds(clips);

      // That's it
    }
  }

*/

  void OnCollisionStay(Collision collision)
  {
    // Vector3 velocity = collision.relativeVelocity;
    // The above does not work because the Player is moving, not the hands (though they are in absolute space)
    // Only play audio when the velocity when we're moving
    //Debug.Log(velocity.x + ", " + velocity.y + ", " + velocity.z);
    //Debug.Log(velocity.magnitude);
    /*if (!m_IsMoving)
    {
      if (m_AudioSource.isPlaying)
      {
        m_AudioSource.Stop ();
      }
    }
    else if (m_IsMoving && collision.gameObject.CompareTag("Wall")) // if velocity is not zero
    {
      Debug.Log ("hitting");
      if (!m_AudioSource.isPlaying)
      {
        m_AudioSource.Play ();
      }
    }*/
  }

  void OnCollisionExit(Collision collision)
  {
    /*//Debug.Log ("exiting");
    // We're no longer in this collider so stop playing the sound
    if (m_AudioSource.isPlaying && collision.gameObject.CompareTag("Wall"))
    {
      Debug.Log ("exiting");
      m_AudioSource.Stop ();
    }
    */
  }
}
