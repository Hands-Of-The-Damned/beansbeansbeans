using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManagerPublisher : MonoBehaviour
{
    /* EVENT 1
     * Betting Round Event
     * send current pot ammount, current bet ammount, current betting round, string of current players turn
     */


    public class SendBettingRoundInfoEvent
    {
        public GameManagerPublisher eventSystem;
        public Player player;
        public int currentBet;
        public int currentPot;
        public int currentRound;

        public SendBettingRoundInfoEvent(Player playersTurn, int bet, int pot, int round)
        {
            player = playersTurn;
            currentBet = bet;
            currentPot = pot;
            currentRound = round;
        }
    }

    public static event System.EventHandler<SendBettingRoundInfoEvent> BettingRoundInfo;

    public void SendRoundInfo(Player player, int bet, int pot, int round)
    {
        BettingRoundInfo?.Invoke(this, new SendBettingRoundInfoEvent(player, bet, pot, round));
    }


    /* EVENT 2
     * Send Delt Card Event
     * Send delt card to a player
     */

    public class DealToPlayerEvent
    {
        public GameManagerPublisher eventSystem;
        public Player player;
        public Card card;

        public DealToPlayerEvent(Player playerDeltTo, Card newCard)
        {
            player = playerDeltTo;
            card = newCard;
        }
    }


    public static event System.EventHandler<DealToPlayerEvent> Deal;

    public void DealToPlayer(Player player, Card newCard)
    {
        Deal?.Invoke(this, new DealToPlayerEvent(player, newCard));
    }



    /* EVENT 3
    * Initial Bet: Big Blind 
    * Take bet from big blind
    */

    public class BigBlindBetEvent
    {
        public GameManagerPublisher eventSystem;
        public int bet;
        public Player player;

        public BigBlindBetEvent(Player bigBlind, int betAmount)
        {
            player = bigBlind;
            bet = betAmount;
        }
    }

    public static event System.EventHandler<BigBlindBetEvent> BigBlind;

    public void TakeBigBet(Player player, int bet)
    {
        BigBlind?.Invoke(this, new BigBlindBetEvent(player, bet));
    }



    /* EVENT 4
    * Initial Bet: Small Blind 
    * Take bet from small blind
    */

    public class SmallBlindBetEvent
    {
        public GameManagerPublisher eventSystem;
        public int bet;
        public Player player;

        public SmallBlindBetEvent(Player smallBlind, int betAmount)
        {
            player = smallBlind;
            bet = betAmount;
        }
    }

    public static event System.EventHandler<SmallBlindBetEvent> SmallBlind;

    public void TakeSmallBet(Player player, int bet)
    {
        SmallBlind?.Invoke(this, new SmallBlindBetEvent(player, bet));
    }

    /* EVENT SomeNumber
     * General Major Arcana
     * Probably different events
     * Tell players and GUI what major arcana was played and its effects, send effects to players and GUI
     */





    /* EVENT HANDLE    
     * Player Betting Round Event
     * receive bet amount, if player folded, if player played major arcana
     */

    /* EVENT HANDLE
     * Major Arcana Effects
     * receive effects: force player to fold for a round, deal cards to a player, force a bet increase
     * There will be others
     * May have to be separate events
     */

    /* EVENT HANDLE    
    * Game Start Event
    * receive an array of the players in the game to initialize the game
    */




}

