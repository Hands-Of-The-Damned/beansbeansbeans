using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Start is called before the first frame update

    string[] minorArcanaCardSuits = new string[] { "Pentacles", "Swords", "Wands", "Cups"};
    string[] minorArcanaCardRanks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "1"};
    string[] majorArcana = new string[] { "0", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "XXI" };
    
    //needs to be changed to list for game in order to allow shuffling of remaining cards.
    public List<string> deck;

    public List<string> discardPile;

    //public int deckLocation;

    
    void Start()
    {
        //generate deck on start
        generateDeck();
        
        //shuffle the deck
        shuffle();
        shuffle();
        shuffle(); //this is a dumb way to do it, but I don't want to implement "shuffle deck x amount of times" when shuffling it once works.
/*        deck = new string[78]; 
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
*/
        //reset deck location to 0
//        deckLocation = 0;

        
    }
    //shuffle the deck
    public void shuffle()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int x = (Random.Range(0, deck.Count - 1));
            string temp;
            temp = deck[x];
            deck[x] = deck[i];
            deck[i] = temp;

        }
    }

    //deal a certain number of cards
    public string[] deal(int numToDeal)
    {
        string[] dealtHand = new string[numToDeal];


        dealtHand = deck.GetRange(0, numToDeal).ToArray();
        deck.RemoveRange(0, numToDeal);

        return dealtHand;
    }

    //generate new deck. Only to be used after a round is over.
    public void generateDeck()
    {
        deck.Clear();
        for (int i = 0; i < minorArcanaCardSuits.Length; i++)
        {

            for (int j = 0; j < minorArcanaCardRanks.Length; j++)
            {
                deck.Add(minorArcanaCardRanks[j] + "Of" + minorArcanaCardSuits[i]);
            }
        }
        for (int i = 0; i < majorArcana.Length; i++)
        {
            deck.Add(majorArcana[i]);
        }
    }

    // Code below is for testing purposes
    /* void Update()
    {
        //test code - to be transitioned for prod
        
        //show deck - t
        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < deck.Count; i++)
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
    */


}
