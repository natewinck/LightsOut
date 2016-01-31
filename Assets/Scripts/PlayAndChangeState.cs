using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(AudioSource))]
public class PlayAndChangeState : MonoBehaviour {
  public List<AudioClip> clipList;
  public string thenTransitionTo;

  IEnumerator Start () {
    var audio = GetComponent<AudioSource>();

    // Play all the clips in sequence
    foreach (var clip in clipList) {
      audio.PlayOneShot(clip);
      yield return new WaitForSeconds(clip.length + 0.5f);
    }

    // Change game state (usually "nextlevel")
    GameManager.instance.gameState.TransitionTo(thenTransitionTo);
  }

  void Update () {

  }
}
