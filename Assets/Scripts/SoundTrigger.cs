using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent (typeof(AudioSource))]
public class SoundTrigger : MonoBehaviour {
  public bool IsOneShot;
  public List<AudioClip> SoundBank;

  private ShuffleDeck<AudioClip> soundDeck;
  private AudioSource source;

  // Use this for initialization
  void Awake () {
    soundDeck = new ShuffleDeck<AudioClip>(SoundBank);
    source = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update () {

  }

  void OnTriggerEnter(Collider c) {
    //Debug.Log ("Hello");
    if (c.CompareTag("Player") && !source.isPlaying) {
      // Pull a random clip from the list of sounds
      var clip = soundDeck.Draw();

      if (clip == null) {
        if (IsOneShot) DisableSelf();
      } else {
        // Replace the AudioSource component with the random clip and play
        source.clip = clip;
  	    source.Play();

        if (IsOneShot) StartCoroutine(DisableAfter(clip.length));
      }
    }
  }

  IEnumerator DisableAfter(float time) {
    yield return new WaitForSeconds(time);
    DisableSelf();
  }

  void DisableSelf() {
    gameObject.SetActive(false);
  }
}
