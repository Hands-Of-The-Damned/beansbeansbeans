using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{

    public enum Face { JACK, QUEEN, KING, ACE, NONE}
    public enum Suit { SPADES, CLUBS, HEARTS, DIAMONDS}

    public int value { get; private set; }
    public Suit suit { get; private set; }
    public Face face { get; private set; }

    public Card(Suit suit, int value)
    {
        if (value > 14 || value < 1) throw new UnityException("Card values must be between 1 and 14!");

        this.value = value;
        this.suit = suit;

        if (value == 1) face = Face.ACE;
        else face = (Face)(value % 10);
    }


}
