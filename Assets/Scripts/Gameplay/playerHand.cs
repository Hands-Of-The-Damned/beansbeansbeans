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
        if(card.Length > 5)
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
}
