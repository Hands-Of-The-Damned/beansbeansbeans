using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsListener : MonoBehaviour
{
    Player player;

    //Subscribe to events
    public void OnEnable()
    {
        GameManagerPublisher.Deal += GameManagerPublisher_Deal;
        GameManagerPublisher.BettingRoundInfo += GameManagerPublisher_BettingRoundInfo;
        GameManagerPublisher.BigBlind += GameManagerPublisher_BigBlindBetEvent;
        GameManagerPublisher.SmallBlind += GameManagerPublisher_SmallBlindBetEvent;
    }


    private void GameManagerPublisher_Deal(object sender, GameManagerPublisher.DealToPlayerEvent args)
    {
        if (args.player == player)
        {
            //recive the card
            player.hand.hand.Add(args.card);
        }
    }

    private void GameManagerPublisher_BettingRoundInfo(object sender, GameManagerPublisher.SendBettingRoundInfoEvent args)
    {
        if (args.player == player)
        {
            //Recive round info and let this player play their hand
            player.currentBetToMatch = args.currentBet;
            player.currentPot = args.currentPot;
            player.checkCurrentRound(args.currentRound);
        }
    }


    public void GameManagerPublisher_BigBlindBetEvent(object sender, GameManagerPublisher.BigBlindBetEvent args)
    {
        if(args.player == player)
        {
            player.blindBet(args.bet);
        }
    }

    public void GameManagerPublisher_SmallBlindBetEvent(object sender, GameManagerPublisher.SmallBlindBetEvent args)
    {
        if(args.player == player)
        {
            player.blindBet(args.bet);
        }
    }

    //Unsubscribe to events
    public void OnDisable()
    {
        GameManagerPublisher.Deal -= GameManagerPublisher_Deal;
        GameManagerPublisher.BettingRoundInfo -= GameManagerPublisher_BettingRoundInfo;
        GameManagerPublisher.BigBlind -= GameManagerPublisher_BigBlindBetEvent;
        GameManagerPublisher.SmallBlind -= GameManagerPublisher_SmallBlindBetEvent;
    }
}
