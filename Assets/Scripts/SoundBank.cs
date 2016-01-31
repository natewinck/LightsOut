using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundBank : MonoBehaviour
{
  public static readonly string WALLBRUSHES = "wallbrushes";
  public static readonly string WALLBUMPS = "wallbumps";
  public static readonly string FOOTSTEPS = "footsteps";
  public static readonly string MUTTERS = "mutters";

  [System.Serializable]
  public class NamedClips {
    public string name;
    public List<AudioClip> clipList;
  }
  public List<NamedClips> SoundList;

  private Dictionary<string, ShuffleDeck> _soundDecks;

  void Start () {
    _soundDecks = new Dictionary<string, ShuffleDeck>();
    foreach (var namedClip in SoundList) {
      _soundDecks[namedClip.name] = new ShuffleDeck(namedClip.clipList);
    }
  }

  public AudioClip Draw (string kind) {
    return (AudioClip) _soundDecks[kind].Draw();
  }

  public List<AudioClip> Draw(string kind, int count)
  {
    List<AudioClip> clips = _soundDecks[kind].Draw(count).Cast<AudioClip>().ToList();

    return clips;
  }

  public List<AudioClip> DrawAll (string kind) {
    return _soundDecks[kind].DrawAll().Cast<AudioClip>().ToList();
  }

  // Update is called once per frame
  void Update () {

  }
}

