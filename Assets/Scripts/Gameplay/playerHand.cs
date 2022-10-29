using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        deck = GameObject.FindObjectOfType<Deck>();

    }
    private bool isMinor(string card)
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
            Debug.Log("Used " + card);
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

    public string Reverse(string cardToReverse)
    {
        if(isMinor(cardToReverse) == false)
        {
            if (isReversed(cardToReverse))
            {
                cardToReverse = cardToReverse.Remove(cardToReverse.IndexOf('R'));
                return cardToReverse;
            }
            else
            {
                cardToReverse = cardToReverse + 'R';
                return cardToReverse;
            }
        }
        else
        {
            return cardToReverse;
        }
    }

    public bool isReversed(string cardToCheck)
    {
              
        return cardToCheck.EndsWith('R');
        
    }
    public void drawCardsFromDeck(int NumToDraw)
    {
        string [] cards = deck.deal(NumToDraw).ToArray<string>();
        hand.AddRange(cards);
    }
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Drawing Cards...");
            drawCardsFromDeck(5);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Showing Cards in Hand...");
            foreach (var card in hand)
            {
                Debug.Log(card);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reversing all cards in hand...");
                for(int i = 0; i < hand.Count; i++)
                {
                    hand[i] = Reverse(hand[i]);
                }
            
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Discard Cards...");
            int x = hand.Count; 
            for(int i = 0; i < x; i++)
            {
                //discard all algo
                discardCard(hand[x - (i + 1)]);

            }

            
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Using All Cards...");
            int x = hand.Count;
            for (int i = 0; i < x; i++)
            {
               //use all algo
               useCard(hand[x - (i + 1)]);

            }


        }

    }
    */


}
