using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Start is called before the first frame update

    string[] minorArcanaCardSuits = new string[] { "Pentacles", "Swords", "Wands", "Cups"};
    string[] minorArcanaCardRanks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Page", "Knight", "Queen", "King", "Ace"};
    string[] majorArcana = new string[] { "0", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "XXI" };
    string[] deck;

    public int deckLocation;

    
    void Start()
    {
        //generate deck on start
        deck = new string[78]; 
        deckLocation = 0;
        for(int i = 0; i < minorArcanaCardSuits.Length; i++)
        {

            for(int j = 0; j < minorArcanaCardRanks.Length; j++)
            {
                deck[deckLocation] = minorArcanaCardRanks[j] + "Of" + minorArcanaCardSuits[i];
                deckLocation++;
            }
        }
        for (int i = 0; i < majorArcana.Length; i++)
        {
            deck[deckLocation] = majorArcana[i];
            deckLocation++;
        }
        //reset deck location to 0
        deckLocation = 0;

        //shuffle the deck
        shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug code - to be transitioned for prod

        //show deck - t
        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < deck.Length; i++)
            {
                Debug.Log(deck[i]);
            }
            
        }
        //shuffle - s
        if (Input.GetKeyDown(KeyCode.S))
        {
            shuffle();
            Debug.Log(deck);
        }
        //deal - d
        if (Input.GetKeyDown(KeyCode.D))
        {
            string[] cards = deal(5);

            Debug.Log("Example Deal");

            for(int i = 0; i < cards.Length; i++)
            {
                Debug.Log(cards[i]);
            }
        }
    }

    //shuffle the deck
    public void shuffle()
    {
        for( int i = 0; i < deck.Length; i++)
        {
            int x = (Random.Range(0, 78 - i));
            swapnum(ref deck[i], ref deck[x]);
        }
    }

    static void swapnum(ref string x, ref string y)
    {
        string temp;
        temp = x;
        x = y;
        y = temp;
        return;
    }
    
    //deal a certain number of cards
    public string[] deal(int numToDeal)
    { 
        string[] dealtHand = new string[numToDeal];
        
        for(int i = 0; i < numToDeal; i++)
        {
            dealtHand[i] = deck[deckLocation];
            deckLocation++;
        }

        return dealtHand;
    }
}
