using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

public class GameManagerPublisher : MonoBehaviour
{
    /* EVENT 1
     * Betting Round Event
     * send current pot ammount, current bet ammount, current betting round, string of current players turn
     

    public class SendBettingRoundInfoEvent
    {
        public GameManagerPublisher eventSystem;
        public Player player;
        int currentBet;
        int currentPot;
        int currentRound;

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
     
    public class DealToPlayerEvent
    {
        public GameManagerPublisher eventSystem;
        public Player player;
        public string card;

        public DealToPlayerEvent(Player playerDeltTo, string cardName)
        {
            player = playerDeltTo;
            card = cardName;
        }
    }


    public static event System.EventHandler<DealToPlayerEvent> Deal;

    public void DealToPlayer(Player player, string cardName)
    {
        Deal?.Invoke(this, new DealToPlayerEvent(player, cardName));
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
    



}
    */
