using UnityEngine;
using System.Collections;

public class ShuffleDeck {
  public int lastReshuffle;
  private ArrayList _cards;
  private int _nextCard;

  public ShuffleDeck(IEnumerable list) {
    _cards = new ArrayList();
    foreach (var item in list) {
      Add(item);
    }
    Reshuffle();
  }

  public ShuffleDeck() {
    _cards = new ArrayList();
  }

  public int Count {
    get { return _cards.Count; }
  }

  public ShuffleDeck Add(object item) { _cards.Add(item); return this; }
  public ShuffleDeck Remove(object item) { _cards.Remove(item); return this; }
  public ShuffleDeck Sort() { _cards.Sort(); return this; }

  public ShuffleDeck Reshuffle() {
    var newDeck = new ArrayList();
    while (_cards.Count > 0) {
      var idx = Random.Range(0, _cards.Count);
      newDeck.Add(_cards[idx]);
      _cards.RemoveAt(idx);
    }
    _cards = newDeck;
    _nextCard = 0;
    lastReshuffle++;
    return this;
  }

  public ArrayList Draw(int count) {
    var hand = new ArrayList();
    if (count > _cards.Count) count = _cards.Count;

    while (hand.Count < count) {
      var card = Draw();
      hand.Add(card);
    }

    return hand;
  }

  public object Draw() {
    if (_cards.Count == 0) return null;

    if (_nextCard >= _cards.Count) {
      Reshuffle();
    }

    return _cards[_nextCard++];
  }
}
