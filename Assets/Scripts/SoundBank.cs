using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundBank : MonoBehaviour
{
  public List<AudioClip> SoundClips;
  //public string name;
  // Instead of using name, I'm cheating and making brushing sounds first
  // and bumping sounds second, always.

  private ShuffleDeck m_SoundDeck;

  // Use this for initialization
  void Start () {
    m_SoundDeck = new ShuffleDeck(SoundClips);
  }

	public AudioClip Draw () {
		return (AudioClip) m_SoundDeck.Draw();
	}

  public List<AudioClip> Draw(int count)
  {
    List<AudioClip> clips = m_SoundDeck.Draw (count).Cast<AudioClip>().ToList();

    return clips;
  }

  public List<AudioClip> DrawAll () {
    return (List<AudioClip>) m_SoundDeck.DrawAll().Cast<AudioClip>().ToList();
  }

  // Update is called once per frame
  void Update () {

  }
}

