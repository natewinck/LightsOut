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
  public static readonly string INTROS = "intros";
  public static readonly string WINS = "wins";
  public static readonly string LOSSES = "losses";

  [System.Serializable]
  public class NamedClips {
    public string name;
    public List<AudioClip> clipList;
  }
  public List<NamedClips> SoundList;

  private Dictionary<string, ShuffleDeck<AudioClip>> _soundDecks;

  void Awake () {
    _soundDecks = new Dictionary<string, ShuffleDeck<AudioClip>>();
    foreach (var namedClip in SoundList) {
      _soundDecks[namedClip.name] = new ShuffleDeck<AudioClip>(namedClip.clipList);
    }
  }

  public AudioClip Draw (string kind) {
    var clips = Draw(kind, 1);
    return clips.Count > 0 ? clips.First() : null;
  }

  public List<AudioClip> Draw(string kind, int count)
  {
    if (!_soundDecks.ContainsKey(kind)) return new List<AudioClip>();
    return _soundDecks[kind].Draw(count);
  }

  public List<AudioClip> DrawAll (string kind) {
    if (!_soundDecks.ContainsKey(kind)) return new List<AudioClip>();
    return _soundDecks[kind].DrawAll();
  }

  // Update is called once per frame
  void Update () {

  }
}

