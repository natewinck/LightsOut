using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundBank : MonoBehaviour
{
  public List<AudioClip> SoundClips;

  private ShuffleDeck m_SoundDeck;

  // Use this for initialization
  void Start () {
    m_SoundDeck = new ShuffleDeck(SoundClips);
  }
	AudioClip Draw () {
		return (AudioClip) m_SoundDeck.Draw();
	} 

  // Update is called once per frame
  void Update () {

  }
}

