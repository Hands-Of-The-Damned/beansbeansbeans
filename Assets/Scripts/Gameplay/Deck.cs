using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Start is called before the first frame update

    string[] minorArcanaCardSuits = new string[] { "Pentacles", "Swords", "Wands", "Cups"};
    string[] minorArcanaCardRanks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Page", "Knight", "Queen", "King"};
    string[] majorArcana = new string[] { "0", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "XI" };
    string[] deck = new string[78];
    public int deckLocation = 0;
    void Start()
    {
        //generate deck on start
        
        for(int i = 0; i < minorArcanaCardSuits.Length; i++)
        {
            for(int j = 0; j < minorArcanaCardRanks.Length; j++)
            {
                deck[deckLocation] = minorArcanaCardSuits[i] + "Of" + minorArcanaCardRanks[j];
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shuffle()
    {

    }
}
