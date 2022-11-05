using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> _allCards = new List<Card>();
    private List<Card> _remainingCards = new List<Card>();

    public Deck()
    {
        for (int suit = 0; suit < 4; suit++)
            for (int value = 1; value <= 14; value++)
                _allCards.Add(new Card((Card.Suit) suit, value));

        _remainingCards.AddRange(_allCards);
    }


    public Card DrawRandomCard(bool removeFromDeck)
    {
        int randomCardIndex = Random.Range(0, _remainingCards.Count);
        Card card = _remainingCards[randomCardIndex];

        if (removeFromDeck)
            _remainingCards.Remove(card);

        return card;
    }
}
