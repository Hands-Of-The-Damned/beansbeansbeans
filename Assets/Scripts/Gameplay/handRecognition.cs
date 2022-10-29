using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using UnityEngine;

public class handRecognition : MonoBehaviour
{
    //Might be better to have playerHand have their own "HandPower" variable, that calls this after every turn, to communicate to the player the poker hands they have.
    //Wilds still need to be implemented throughout the code.


    //This script will need to do the following:
    //Get player hands at the table
    //Rank a set of player hands
    //Implement following hands: (ranking is highest(10) to lowest (1))
    //Royal Flush, Straight Flush (straight + flush), Four-of-a-kind, Full House (Two-pair + three of a kind), Flush, Straight, Three of a kind, Two pair, Pair, High Card
    //need to check for rank and suits, mainly for easier processing
    
    //lib to check for straights
    int[][] straightLib = new int[][] { 
        new int [] { 1, 2, 3, 4, 5 },
        new int [] { 2, 3, 4, 5, 6 },
        new int [] { 3, 4, 5, 6, 7 },
        new int [] { 4, 5, 6, 7, 8 },
        new int [] { 5, 6, 7, 8, 9 },
        new int [] { 6, 7, 8, 9, 10 },
        new int [] { 7, 8, 9, 10, 11 },
        new int [] { 8, 9, 10, 11, 12 },
        new int [] { 9, 10, 11, 12, 13 },
        new int [] { 10, 11, 12, 13, 14 },
        new int [] { 11, 12, 13, 14, 1 },
        new int [] { 12, 13, 14, 1, 2 },
        new int [] { 13, 14, 1, 2, 3 },
        new int [] { 14, 1, 2, 3, 4 } 
    };

    private int[] _handRankingPrimary;
    private int[] _handRankingSecondary;

    public playerHand[] playerHands;

    //may be a good idea to make a card class now ._. 
    //But I'm lazy as hell, if it's necessary or we see a drop in performance, I'll consider it.
    private string[][] _handsToBeChecked;
    
    //1 - Ace, 11 - Page, 12 - Knight, 13 - Queen, 14 - King
    private int[][] _cardRank;
    //1 - Pentacles, 2 - Swords, 3 - Wands, 4 - Cups
    private int[][] _cardSuits;
    private void Start()
    {
        playerHands = GameObject.FindObjectsOfType<playerHand>();


    }

    //need to get cards to actually figure out if they are correct.
    void getPokerCards()
    {        
        _handsToBeChecked = new string[playerHands.Length][];
        for(int i = 0; i < playerHands.Length; i++)
        {
            _handsToBeChecked[i] = playerHands[i].getMinor();
        }

        //find ranking + suits
        for(int i = 0; i < _handsToBeChecked.Length; i++)
        {
            for(int j = 0; j < _handsToBeChecked[i].Length; j++)
            {
                //split card ID in half based off of Of keyword
                var splitcard = _handsToBeChecked[i][j].Split("Of");
                
                //get cardranks
                _cardRank[i][j] = IntParseFast(splitcard[0]);

                //get card suits
                switch (splitcard[1])
                {
                    case "Pentacles":
                        _cardSuits[i][j] = 1;
                        break;

                    case "Swords":
                        _cardSuits[i][j] = 2;
                        break;

                    case "Wands":
                        
                        _cardSuits[i][j] = 3;
                        break;

                    case "Cups":
                        _cardSuits[i][j] = 4;
                        break;

                    default:
                        Debug.Log("Something is very very wrong.");
                        break;

                }
            }
        }

        //sort rankings + suits to make finding straight/flush easier.
        for (int i = 0; i < _handsToBeChecked.Length; i++)
        {
            Array.Sort(_cardSuits[i]);
            Array.Sort(_cardRank[i]);
        }
    }

    //royal flush gets it's own check
    bool RoyalFlushCheck(int[] suits, int[] ranks)
    {
        //check if all suits are the same.
        if((suits == new int[] {11, 12, 13, 14, 1}) && (ranks.Distinct().Count() == 1)){
            return true;
        }
        else
        {
            return false;
        }
    }

    //check for straights
    bool StraightCheck(int[] suits)
    {
              
        for(int i = 0; i < straightLib.Length; i++)
        {
            if(suits == straightLib[i])
            {
                return true;
            }
            
        }
        return false;
    }

    //check for flush
    bool FlushCheck(int[] ranks)
    {
        if (ranks.Distinct().Count() == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //check for both flush and straight
    bool StraightFlush(int[] suits, int[] ranks)
    {
        if (StraightCheck(suits) && FlushCheck(ranks))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    bool FourOfAKindCheck(int[] ranks)
    {
        bool flag = false;
        for(int i = 1; i <= 14; i++)
        {
            int count = 0;
            for(int j = 0; j < ranks.Length; j++)
            {
                if (ranks[j] == i)
                {
                    count++;
                }

                if (count == 4)
                {
                    flag = true;
                }
            }
        }

        return flag;
    }

    //faster int.parse code
    public static int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;
    }
}
