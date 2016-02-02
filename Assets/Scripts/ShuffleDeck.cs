using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Easy collections of objects that won't repeat until they've all been used.
public class ShuffleDeck<T> {
  public struct ShuffleState {
    public int shuffleCount;
    public int seed;
    public int nextCardNum;
  }
  // SAVE AND RESTORE STATE IS UNTESTED
  public ShuffleState State {
    get { return _state; }
    set {
      _state = value;
      _rng = new System.Random(_state.seed);

      // Prime the RNG sequence. Fast-forward by number of cards drawn.
      var decksDrawnCount = _cards.Count * _state.shuffleCount;
      for (var i = decksDrawnCount; i >= 0; i--) {
        _rng.Next();
      }
      Reshuffle(_rng);
    }
  }
  public int Count { get { return _cards.Count; } }

  private ShuffleState _state;
  private List<T> _cards;
  private System.Random _rng;

  public ShuffleDeck() {
    _cards = new List<T>();
  }
  public ShuffleDeck(IEnumerable<T> list) {
    _cards = new List<T>(list);
    _state.seed = Random.Range(int.MinValue, int.MaxValue);
    _rng = new System.Random(_state.seed);
    Reshuffle(_rng);
  }

  public ShuffleDeck<T> Add(T item) { _cards.Add(item); return this; }
  public ShuffleDeck<T> Remove(T item) { _cards.Remove(item); return this; }
  public ShuffleDeck<T> Sort() { _cards.Sort(); return this; }

  public void SetSeed(int seed) {
    _state.seed = seed;
    _rng = new System.Random(_state.seed);
  }

  public ShuffleDeck<T> Reshuffle() {
    return Reshuffle(_rng);
  }
  public ShuffleDeck<T> Reshuffle(System.Random shuffleRNG) {
    _state.nextCardNum = 0;
    _state.shuffleCount++;

    if (_cards.Count <= 1) return this;

    var lastCard = _cards.Last();

    for(var i = 0; i < _cards.Count; i++) {
      var j = shuffleRNG.Next(i, _cards.Count);
      var temp = _cards[i];
      _cards[i] = _cards[j];
      _cards[j] = temp;
    }

    // If the first card of the new deck is the last card of the old deck,
    // reshuffle to avoid hitting the same card twice in a row across
    // shuffles. Not 100% foolproof. Not good for actual card decks. Expensive?
    // Friendly to save/restore state.
    if (EqualityComparer<T>.Default.Equals(_cards.First(), lastCard)) {
      return Reshuffle();
    }

    return this;
  }

  public List<T> DrawAll() {
    return Draw (_cards.Count);
  }

  public List<T> Draw(int count) {
    var hand = new List<T>();
    if (count > _cards.Count) count = _cards.Count;

    while (hand.Count < count) {
      var card = Draw();
      hand.Add(card);
    }

    return hand;
  }

  public T Draw() {
    if (_cards.Count == 0) return default(T);

    if (_state.nextCardNum >= _cards.Count) {
      Reshuffle();
    }
Debug.Log("drawing card " + _state.nextCardNum + " of " + _cards.Count);
    return _cards[_state.nextCardNum++];
  }
}
