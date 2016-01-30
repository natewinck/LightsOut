using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundTrigger : MonoBehaviour {
  public List<AudioClip> SoundBank;

  private ShuffleDeck soundDeck;

  // Use this for initialization
  void Start () {
    soundDeck = new ShuffleDeck(SoundBank);
  }

  // Update is called once per frame
  void Update () {

  }

  void OnTriggerEnter(Collider c) {
  }
}
