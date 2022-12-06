//Michael "Mickey" Kerr
//2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Card
{

        ///<summary> 
        ///This covers Minor and Major arcana
        ///</summary>
    public string CardName;
    public bool IsMinor;
    public bool IsReverse = false;
    public bool IsRankWild = false;
    public bool IsSuitWild = false;
    public virtual void ReverseCard() { }
}

public class MinorArcana : Card
{
    //public string CardName;
    //public new bool isMinor;
    public int CardRank;
    public int CardSuit;
    public MinorArcana(int cardRank, int cardSuit)
    {
            CardRank = cardRank;
            CardSuit = cardSuit;
            IsMinor = true;

            string CardNameP1;
            string CardNameP2;

            switch (CardRank)
            {
                case 1:
                    CardNameP1 = "Ace";
                    break;

                case 11:
                    CardNameP1 = "Page";
                    break;

                case 12:
                    CardNameP1 = "Knight";
                    break;

                case 13:
                    CardNameP1 = "Queen";
                    break;

                case 14:
                    CardNameP1 = "King";
                    break;

                default:
                    CardNameP1 = CardRank.ToString();
                    break;
            }

            //1 - Pentacles, 2 - Swords, 3 - Wands, 4 - Cups
            switch (CardSuit)
            {
                case 1:
                    CardNameP2 = "Pentacles";
                    break;

                case 2:
                    CardNameP2 = "Swords";
                    break;

                case 3:
                    CardNameP2 = "Wands";
                    break;
                
                case 4:
                    CardNameP2 = "Cups";
                    break;

                default :
                    CardNameP2 = "UNHANDLED EXPECTION";
                    break;
            }

            CardName = CardNameP1 + " of " + CardNameP2;
        }

    }

public class MajorArcana : Card
{
    //public new bool isMinor;
    //public new string CardName;
    public string Numeral;
    public int CardNumber;
    /// <summary>
    /// Only need number to reference Major Arcana Effect Table.
    /// </summary>
    public string CardEffect;
    public MajorArcana(int cardNumber)
    {
        CardNumber = cardNumber;
        IsMinor = false;
        IsReverse = false;
        //CardEffect Reference table will need to be added in the future, use placeholder for now
        CardEffect = "NULL";

        //this is super gross, but should just act as a lookup table so generate deck will only need basic info.
        switch (CardNumber)
        {
            case 0:
                CardName = "The Fool";
                Numeral = "0";
                break;

            case 1:
                CardName = "The Magician";
                Numeral = "I";
                break;

            case 2:
                CardName = "The High Priestess";
                Numeral = "II";
                break;

            case 3:
                CardName = "The Empress";
                Numeral = "III";
                break;

            case 4:
                CardName = "The Emperor";
                Numeral = "IV";
                break;

            case 5:
                CardName = "The Hierophant";
                Numeral = "V";
                break;

            case 6:
                CardName = "The Lovers";
                Numeral = "VI";
                break;

            case 7:
                CardName = "The Chariot";
                Numeral = "VII";
                break;

            case 8:
                CardName = "Strength";
                Numeral = "VIII";
                break;

            case 9:
                CardName = "The Hermit";
                Numeral = "IX";
                break;

            case 10:
                CardName = "Wheel of Fortune";
                Numeral = "X";
                break;

            case 11:
                CardName = "Justice";
                Numeral = "XI";
                break;

            case 12:
                CardName = "The Hanged Man";
                Numeral = "XII";
                break;

            case 13:
                CardName = "Death";
                Numeral = "XIII";
                break;

            case 14:
                CardName = "Temperance";
                Numeral = "XIV";
                break;

            case 15:
                CardName = "The Devil";
                Numeral = "XV";
                break;

            case 16:
                CardName = "The Tower";
                Numeral = "XVI";
                break;

            case 17:
                CardName = "The Star";
                Numeral = "XVII";
                break;

            case 18:
                CardName = "The Moon";
                Numeral = "XVIII";
                break;

            case 19:
                CardName = "The Sun";
                Numeral = "XIX";
                break;

            case 20:
                CardName = "Judgement";
                Numeral = "XX";
                break;

            case 21:
                CardName = "The World";
                Numeral = "XXI";
                break;

        }
        
    }

    public override void ReverseCard()
    {
        IsReverse = !IsReverse;
    }
}