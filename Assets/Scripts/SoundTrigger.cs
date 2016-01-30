using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent (typeof(AudioSource))]
public class SoundTrigger : MonoBehaviour {
  public List<AudioClip> SoundBank;

  private ShuffleDeck soundDeck;
  private AudioSource source;

  // Use this for initialization
  void Start () {
    soundDeck = new ShuffleDeck(SoundBank);
    source = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update () {

  }

  void OnTriggerEnter(Collider c) {
    if (c.CompareTag("Player")) {
      var clip = (AudioClip) soundDeck.Draw();
      source.PlayOneShot(clip);
    }
  }
}
