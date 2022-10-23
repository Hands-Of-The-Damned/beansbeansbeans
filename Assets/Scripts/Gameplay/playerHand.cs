using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerHand : MonoBehaviour
{
    //Player hand
    public List<string> hand;

    public Deck deck;

    public UnityEvent playCard;

    // Start is called before the first frame update
    private void Start()
    {
        //get deck component to make life easier
        deck = GetComponent<Deck>();
    }
    // Update is called once per frame


    public bool isMinor(string card)
    {
        if(card.Length > 6)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //can only use major arcana
    public void useCard(string card)
    {
        if (isMinor(card) == false) 
        {
            playCard?.Invoke();
            discardCard(card); 
        }

    }

    //get rid of card, add to discard deck
    public void discardCard(string card)
    {
        hand.Remove(card);
        deck.discardPile.Add(card);
    }
    
    //get minor arcana card
    public string[] getMinor()
    {
        List<string> minorArcana = new List<string>();
        for (int i = 0; i < hand.Count; i++)
        {
            if (isMinor(hand[i]))
            {
                minorArcana.Add(hand[i]);
            }
        }   

        return minorArcana.ToArray();
    }

    //Add R to end if no R is there. If R is there, remove R. Might be broken, needs review

    public void reverse(ref string cardToReverse)
    {
        if(isMinor(cardToReverse) == false)
        {
            if (cardToReverse.EndsWith('R'))
            {
                cardToReverse = cardToReverse.Remove(cardToReverse.IndexOf('R'));
            }
        }
        else
        {
            cardToReverse = cardToReverse + 'R';
        }
    }

    public bool isReversed(string cardToCheck)
    {
              
        return cardToCheck.EndsWith('R');
        
    }
    public void drawCardsFromDeck(int NumToDraw)
    {
        List<string> cards = new List<string>(deck.deal(NumToDraw));
        hand.AddRange(cards);
    }
}
