//Michael "Mickey" Kerr
//2022

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class playerHand : MonoBehaviour
{
    //Player hand
    public List<Card> hand = new List<Card>();
    public List<ChangeMaterial> handDisplays = new(5);

    public Deck deck;

    public UnityEvent playCard;

    public handRecognition handRecognition;

    // Start is called before the first frame update
    private void Start()
    {
        //get deck component to make life easierh
        deck = GameObject.FindObjectOfType<Deck>();
        handRecognition = GameObject.FindObjectOfType<handRecognition>();

    }

    //can only use major arcana
    public void useCard(Card card)
    {
        if (card.IsMinor == false) 
        {
            //Do a thing
            discardCard(card);
        }

    }

    //get rid of card, add to discard deck
    public void discardCard(Card card)
    {
        hand.Remove(card);
        deck.discardPile.Add(card);
    }
    
    //get minor arcana card
    public Card[] getMinor()
    {
        List<MinorArcana> minorArcana = new List<MinorArcana>();
        
        minorArcana.AddRange(hand.OfType<MinorArcana>().ToArray());

        return minorArcana.ToArray();
    }

    public void drawCardsFromDeck(int NumToDraw)
    {
        Card [] cards = deck.deal(NumToDraw);
        hand = cards.ToList<Card>();
        
    }
    


    /// <summary>
    /// Code that is depreciated is below. Mainly driver code
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Drawing Cards...");
            drawCardsFromDeck(5);
            for (int i = 0; i < 5; i++)
            {
                if (hand[i] is MinorArcana)
                    handDisplays[i].GetComponent<Renderer>().material = handDisplays[i].cardMat[(hand[i] as MinorArcana).CardRank];
            }
        }

        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    Debug.Log("Showing Cards in Hand...");
        //    foreach (var card in hand)
        //    {
        //        Debug.Log(card.CardName + ' ' + card.IsReverse);

        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Debug.Log("Reversing all cards in hand...");
        //        for(int i = 0; i < hand.Count; i++)
        //        {
        //            hand[i].ReverseCard();
        //        }

        //}

        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Debug.Log("Discard Cards...");
        //    int x = hand.Count; 
        //    for(int i = 0; i < x; i++)
        //    {
        //        //discard all algo
        //        discardCard(hand[x - (i + 1)]);

        //    }


        //}

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    Debug.Log("Using All Cards...");
        //    int x = hand.Count;
        //    for (int i = 0; i < x; i++)
        //    {
        //       //use all algo
        //       useCard(hand[x - (i + 1)]);

        //    }

        //}

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    int[] arr = new int[3];
        //    arr = handRecognition.HandRecognition(this);
        //    Debug.Log("Card Hand Value:" + arr[0] + " " + arr[1] + " " + arr[2]);
        //}

    }
    


}
