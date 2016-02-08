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
  public AudioSource m_MutterAudioSource;
  public AudioSource m_MusicAudioSource;
  public AudioClip m_WinMusic;
  public AudioClip m_LoseMusic;

  private GameManager m_GameManager;
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
    m_GameManager = GameManager.instance;
    m_StateMachine = m_GameManager.gameState;
    m_StateMachine.OnTransition("playing", "win", OnWin);
    m_StateMachine.OnTransition("penalty", "win", OnWin);
    m_StateMachine.On("lose", OnLose);
    m_StateMachine.On("intro", OnIntro);
    m_StateMachine.OnTransition("penalty", "playing", AfterPenalty);
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

  void OnTriggerEnter(Collider other) {
    Debug.Log (other.name);
    var otherSoundBank = other.GetComponent<SoundBank>();

    CheckPenalty(otherSoundBank, other);

    if (otherSoundBank != null && !m_MutterAudioSource.isPlaying) {
      PlayMutter(otherSoundBank, other);
    }

    CheckWin(other);
  }

  void AfterPenalty(string newState) {
    var penaltyCount = m_GameManager.penaltyCount;

    // Check for soundbank clip type "penalty1", "penalty2"
    var clip = m_SoundBank.Draw("penalties" + penaltyCount);
    if (clip != null) {
      m_MutterAudioSource.clip = clip;
      m_MutterAudioSource.PlayDelayed(0.25f);
    }
  }

  void PlayMutter(SoundBank otherSoundBank, Collider other) {
    // Get a mutter if exists.
    var clip = otherSoundBank.Draw(SoundBank.MUTTERS);
    if (clip == null) return;

    // Get the child of this (which should be the hand) and add this audio clip to it, then play
    m_MutterAudioSource.clip = clip;
    m_MutterAudioSource.PlayDelayed(0.5f);

    if (m_StateMachine.GetState() == "penalty") {
      Debug.Log("delaying penalty to playing");
      StartCoroutine(m_StateMachine.DelayedTransitionTo("playing", clip.length + 0.5f));
    }
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
      m_MutterAudioSource.clip = clip;
      m_MutterAudioSource.PlayDelayed(0.5f);
      StartCoroutine(m_StateMachine.DelayedTransitionTo("playing", 0.5f + clip.length));
    }
  }

  void OnWin(string oldState) {
    // Get a mutter if exists.
    var clip = m_SoundBank.Draw(SoundBank.WINS);
    if (clip == null) {
      m_StateMachine.TransitionTo("nextlevel");
    } else {
      m_MutterAudioSource.clip = clip;
      m_MusicAudioSource.clip = m_WinMusic;
      m_MutterAudioSource.PlayDelayed(0.5f);
      m_MusicAudioSource.PlayDelayed(0.5f);
      StartCoroutine(m_StateMachine.DelayedTransitionTo("nextlevel", m_WinMusic.length + 0.5f));
    }
  }

  void OnLose(string oldState) {
    // Get a mutter if exists.
    var clip = m_SoundBank.Draw(SoundBank.LOSSES);
    if (clip == null) {
      m_StateMachine.TransitionTo("replaylevel");
    } else {
      m_MutterAudioSource.clip = clip;
      m_MusicAudioSource.clip = m_LoseMusic;
      m_MutterAudioSource.PlayDelayed(0.5f);
      m_MusicAudioSource.PlayDelayed(0.5f);
      StartCoroutine(m_StateMachine.DelayedTransitionTo("replaylevel", m_LoseMusic.length));
    }
  }

}
