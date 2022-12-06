//Michael "Mickey" Kerr
//2022

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class MickeyDemo : MonoBehaviour
{
    public playerHand playerHand;
    public Deck deck;
    public handRecognition handRecognition;

    private void Start()
    {
        deck = GameObject.FindObjectOfType<Deck>();
        playerHand = GameObject.FindObjectOfType<playerHand>();
        handRecognition = GameObject.FindObjectOfType<handRecognition>();
    }

    public void DrawCardTest(int cardAmt)
    {
        Debug.Log("Drawing Cards...");
        playerHand.drawCardsFromDeck(cardAmt);
    }

    public void ShowCardsTest()
    {
        Debug.Log("Showing Cards in Hand...");
        Debug.Log("Amount of cards in hand: " + playerHand.hand.Count.ToString());
        string chonk = "";
        foreach (var card in playerHand.hand)
        {
            char Reverse = 'N';
            if (card.IsReverse)
                Reverse = 'R';
            chonk = chonk + card.CardName + ' ' + Reverse + ", ";

        }
        Debug.Log(chonk);
    }

    public void ReverseCardsTest()
    {
        Debug.Log("Reversing all cards in hand...");
        for (int i = 0; i < playerHand.hand.Count; i++)
        {
            playerHand.hand[i].ReverseCard();
        }
    }

    public void UseCardsTest()
    {
        Debug.Log("Using All Cards...");
        int x = playerHand.hand.Count;
        for (int i = 0; i < x; i++)
        {
            //use all algo
            playerHand.useCard(playerHand.hand[x - (i + 1)]);

        }
    }

    public void HandRecognitionTest()
    {
        string HandValue, HighCardRank, HighCardSuit;
        int[] arr = new int[3];
        arr = handRecognition.HandRecognition(playerHand);

        //Assign handval
        switch (arr[0])
        {
            case 1:
                HandValue = "High Card";
                break;
            case 2:
                HandValue = "Pair";
                break;
            case 3:
                HandValue = "Two Pair";
                break;
            case 4:
                HandValue = "Three of a Kind";
                break;
            case 5:
                HandValue = "Straight";
                break;
            case 6:
                HandValue = "Flush";
                break;
            case 7:
                HandValue = "Full House";
                break;
            case 8:
                HandValue = "Four of a Kind";
                break;
            case 9:
                HandValue = "Straight Flush";
                break;
            case 10:
                HandValue = "Royal Flush";
                break;
            default:
                HandValue = "UNHANDLED EXCEPTION";
                break;
        }

        //assign CardRank
        switch (arr[1])
        {
            case 1:
                HighCardRank = "Ace";
                break;
            case 11:
                HighCardRank = "Page";
                break;
            case 12:
                HighCardRank = "Knight";
                break;
            case 13:
                HighCardRank = "Queen";
                break ;
            case 14:
                HighCardRank = "King";
                break;

            default:
                HighCardRank = arr[1].ToString();
                break;
        }

        switch (arr[2])
        {
            case 1:
                HighCardSuit = "Pentacles";
                break;

            case 2:
                HighCardSuit = "Swords";
                break;

            case 3:
                HighCardSuit = "Wands";
                break;

            case 4:
                HighCardSuit = "Cups";
                break;

            default:
                HighCardSuit = "UNHANDLED EXPECTION";
                break;
        }

        Debug.Log("Hand Recognition Result:");
        Debug.Log("Raw Hand Val: " + arr[0].ToString() + "\tHand Val: " + HandValue);
        Debug.Log("High Card: " + HighCardRank + " of " + HighCardSuit);
    }

    public void DiscardCardsTest()
    {
        Debug.Log("Discarding all cards in hand...");
        int x = playerHand.hand.Count;
        for(int i = 0; i < x; i++)
        {
            playerHand.discardCard(playerHand.hand[x - (i + 1)]);
        }
    }

    public void ShowDeckTest()
    {
        Debug.Log("Showing cards in deck...");
        Debug.Log("Amount of cards in deck: " + deck.deck.Count.ToString());
        string chonk = "";
        for (int i = 0; i < deck.deck.Count; i++)
        {
            chonk = chonk + deck.deck[i].CardName + ", ";
        }

        Debug.Log(chonk);
    }

    public void ShuffleTest()
    {
        deck.shuffle();
        Debug.Log("Shuffled the deck!");
    }

    public void ShowDiscardTest()
    {
        string chonk = "";
        for (int i = 0; i < deck.discardPile.Count; i++)
        {
            chonk = chonk + deck.discardPile[i].CardName + ", ";
        }

        Debug.Log(chonk);
    }

    public void ResetDeckTest()
    {
        Debug.Log("Resetting deck...");

        deck.resetDeck();
    }
}
