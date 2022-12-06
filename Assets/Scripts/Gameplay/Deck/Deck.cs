//Michael "Mickey" Kerr
//2022

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Start is called before the first frame update

    int[] minorArcanaCardSuits = new int[] { 1, 2 , 3 , 4};
    int[] minorArcanaCardRanks = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 ,14};
    int[] majorArcana = new int[] { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21 };
    
    //needs to be changed to list for game in order to allow shuffling of remaining cards.

    public List<Card> deck = new List<Card>();

    public List<Card> discardPile = new List<Card>();

    //public int deckLocation;

    
    void Start()
    {
        generateDeck();
        shuffle();
    }



    //shuffle the deck
    public void shuffle()
    {
        for(int i = 0; i < deck.Count; i++)
        {
            int x = (Random.Range(0, deck.Count - 1));
            Card temp;
            temp = deck[x];
            deck[x] = deck[i];
            deck[i] = temp;
            
        }
    }

  //static void swapnum(string x, string y){    }
    
    //deal a certain number of cards
    public Card[] deal(int numToDeal)
    { 
        Card[] dealtHand = new Card[numToDeal];
        
        //for(int i = 0; i < numToDeal; i++)
        
        dealtHand = deck.GetRange(0, numToDeal).ToArray();
 //           deckLocation++;
        deck.RemoveRange(0, numToDeal);
        return dealtHand;
    }

    public void generateDeck()
    {
        //DEPRECIATED
        //___________

        //deck = new string[78];
        //deckLocation = 0;
        //___________

        for (int i = 0; i < minorArcanaCardSuits.Length; i++)
        {

            for (int j = 0; j < minorArcanaCardRanks.Length; j++)
            {
                deck.Add(new MinorArcana(minorArcanaCardRanks[j], minorArcanaCardSuits[i]));
                //deckLocation++;
            }
        }
        for (int i = 0; i < majorArcana.Length; i++)
        {
            deck.Add(new MajorArcana(majorArcana[i]));
            //deckLocation++;
        }
        //reset deck location to 0
        //deckLocation = 0;
    }

    //Should only be called when absolutely necessary
    public void resetDeck()
    {
        deck.Clear();
        discardPile.Clear();

        generateDeck();
        shuffle();
    }




    /// <summary>
    /// Code below is depreciated, or was utilized for testing purposes
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //Debug code - to be transitioned for prod

        //show deck - t
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    for (int i = 0; i < deck.Count; i++)
        //    {
        //        Debug.Log(deck[i].CardName);
        //    }

        //}
        ////shuffle - s
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    shuffle();
        //    Debug.Log(deck);
        //}
        ////deal - d

        ////if (Input.GetKeyDown(KeyCode.D))
        ////{
        ////    Card[] cards = deal(5);

        ////    Debug.Log("Example Deal");

        ////    for(int i = 0; i < cards.Length; i++)
        ////    {
        ////        Debug.Log(cards[i].CardName);
        ////    }
        ////}

        ////show discads - e

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    for (int i = 0; i < discardPile.Count; i++)
        //    {
        //        Debug.Log(discardPile[i].CardName);
        //    }
        //}
    }
}
