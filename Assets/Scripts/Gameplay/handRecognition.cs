using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class handRecognition : MonoBehaviour
{
    //Might be better to have playerHand have their own "HandPower" variable, that calls this after every turn, to communicate to the player the poker hands they have.
    //Wilds still need to be implemented throughout the code.


    //This script will need to do the following:
    //Get player hands at the table
    //Rank a set of player hands
    //Implement following hands: (ranking is highest(10) to lowest (1))
    //Royal Flush*, Straight Flush (straight + flush), Four-of-a-kind*, Full House (Two-pair + three of a kind), Flush*, Straight*, Three of a kind*, Two pair, Pair*, High Card*
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

    //Represents all possible flushes
    int[][] flushLib = new int[][]
    {
        new int [] {1, 1, 1, 1, 1},
        new int [] {2, 2, 2, 2, 2},
        new int [] {3, 3, 3, 3, 3},
        new int [] {4, 4, 4, 4, 4}
    };

    /// <summary>
    /// Need to get playerHand info, and put it into an array of the same size for manipulation.
    /// </summary>

    public playerHand AnalyzedHand;

    List<MinorArcana> PokerHand = new List<MinorArcana>();
    List<MajorArcana> EffectsHand = new List<MajorArcana>();
    List<int> PokerHandSuits = new List<int>();
    List<int> PokerHandRanks = new List<int>(); 
    int wildRankCount = 0;
    int wildSuitCount = 0;
    void getHandInfo()
    {
        //Get info for major and minor arcana handtypes
        PokerHand.AddRange(AnalyzedHand.hand.OfType<MinorArcana>());
        EffectsHand.AddRange(AnalyzedHand.hand.OfType<MajorArcana>());

        //Sort PokerHand
        PokerHand.Sort((x, y) => x.CardRank.CompareTo(y.CardRank));

        //Get suits and ranks
        for(int i = 0; i < PokerHand.Count; i++)
        {
            PokerHandSuits.Add(PokerHand[i].CardSuit);
            PokerHandRanks.Add(PokerHand[i].CardRank);
        }

        //Get Wilds in hand 
        for(int i = 0; i < EffectsHand.Count; i++)
        {
            if (EffectsHand[i].IsRankWild == true)
            {
                wildRankCount++;
            }
            if (EffectsHand[i].IsSuitWild == true)
            {
                wildSuitCount++;
            }
        }

    }

    /// <summary>
    /// Only accepts a hand of 5. Preprocessing before a royal check must be done to ensure that the Cards passed are passed absolutely correctly, e.g. Will need check directly from player hand.
    /// </summary>
    bool RoyalFlushCheck(int[] suits, int[] ranks)
    {
        int count = 0;
        int AmtSuitToWin = 5;
        int AmtRankToWin = 5;
        int winCond = 5;
        bool suitFlag = false;
        bool rankFlag = false;
        bool flag = false;

        //Lib for Royal Flush Ranks
        int[] royalFlushRanks = new int[] { 11, 12, 13, 14, 1 };

        //Wild handling
        AmtRankToWin = AmtRankToWin - wildRankCount;
        AmtSuitToWin = AmtSuitToWin - wildSuitCount;

        //What if player rank wild and suit wild dont match up? Choose the higher number in royal flush case
        if(AmtRankToWin > AmtSuitToWin)
        {
            winCond = AmtRankToWin;
        }
        if(AmtRankToWin < AmtSuitToWin)
        {
            winCond = AmtSuitToWin;
        }

        //Check flush lib first. It's bigger.
        for (int i = 0;  i < flushLib.Length; i++)
        {
            //reset count to 0
            count = 0;
            for(int j = 0; j < flushLib[i].Length; j++)
            {
                //Compare flushlib against suits and ranks against RoyalFlush
                if((flushLib[i][j] == suits[j]) && (royalFlushRanks[j] == ranks[j]))
                {
                    count++;
                }

                if(count == winCond)
                {
                    flag = true;
                }

            }
        }

        return flag;
    }

    //check for straights
    bool StraightCheck(int[] ranks)
    {
        bool flag = false;
        int check = 0;
        int winCond = 5 - wildRankCount;

        for(int i = 0; i < straightLib.Length; i++)
        {
            check = 0;
            for(int j = 0; j < straightLib[i].Length; j++)
            {
                if(straightLib[i][j] == ranks[j])
                {
                    check++;
                }
                if(check == winCond)
                {
                    flag = true;
                }
            }
            
        }
        return flag;
    }

    //check for flush
    bool FlushCheck(int[] suits)
    {
        bool flag = false;
        int check = 0;
        int winCond = 5 - wildSuitCount;

        for (int i = 0; i < flushLib.Length; i++)
        {
            check = 0;
            for (int j = 0; j < flushLib[i].Length; j++)
            {
                if (flushLib[i][j] == suits[j])
                {
                    check++;
                }
                if (check == winCond)
                {
                    flag = true;
                }
            }
        }
        return flag;
        
    }

    //check for both flush and straight, will need to be refactored, most probably wrong (i.e. what if there's a flush and a straight but they're not the same cards? Check Royal Flush code
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

    //check for four of a kind
    bool FourOfAKindCheck(int[] ranks)
    {
        bool flag = false;
        int winCond = 4 - wildRankCount;
        int count = 0;

        for(int i = 1; i <= 14; i++)
        {
            count = 0;
            for(int j = 0; j < ranks.Length; j++)
            {
                if (ranks[j] == i)
                {
                    count++;
                }

                if (count == winCond)
                {
                    flag = true;
                }
            }
        }

        return flag;
    }

    //check for three of a kind
    bool ThreeOfAKindCheck(int[] ranks)
    {
        bool flag = false;
        int winCond = 3 - wildRankCount;
        int count = 0;

        for (int i = 1; i <= 14; i++)
        {
            count = 0;
            for (int j = 0; j < ranks.Length; j++)
            {
                if (ranks[j] == i)
                {
                    count++;
                }

                if (count == winCond)
                {
                    flag = true;
                }
            }
        }

        return flag;   
    }

    bool PairCheck(int[] ranks)
    {
        bool flag = false;
        int winCond = 2 - wildRankCount;
        int count = 0;
        for (int i = 1; i <= 14; i++)
        {
            count = 0;
            for (int j = 0; j < ranks.Length; j++)
            {
                if (ranks[j] == i)
                {
                    count++;
                }

                if (count == winCond)
                {
                    flag = true;
                }
            }
        }

        return flag;
    }

    //take entire player hand, check for 2 pairs
    bool TwoPairCheck(int[] ranks)
    {
        bool flag = false;
        int rankPair1;
        for(int i = 1; i <= 14; i++)
        {
            int count = 0;
            for (int j = 0; j < ranks.Length; j++)
            {
                if (ranks[j] == i)
                {
                    count++;
                }

                if (count == 2)
                {
                    flag = true;
                }
            }
        }
        return flag;
    }

    //take entire player hand, check for 1 pair, and 1 three of a kind
    bool FullHouseCheck(int[] ranks)
    {
        bool flag = false;

        return flag;
    }
    
    /// <summary>
    /// Please see Card.cs for more information on
    /// </summary>
    /// <param name="playerHand">The player hand that is to be analyzed, and given a ranking.</param>
    /// <returns>Returns a set of three integers [PokerHandRanking, HighCardRank, HighCardSuit]</returns>
    public int[] HandRecognition(playerHand playerHand)
    {
        int PokerHandRanking, HighCardRank, HighCardSuit;

        //PLACEHOLDER
        int[] HandRecognitionReturn = new int[] {0,0,0};

        return HandRecognitionReturn;
    }
}
