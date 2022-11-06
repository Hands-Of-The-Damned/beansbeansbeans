using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsListener : MonoBehaviour
{
    //Subscribe to events
    public void OnEnable()
    {
        GameManagerPublisher.Deal += GameManagerPublisher_Deal;
        GameManagerPublisher.BettingRoundInfo += GameManagerPublisher_BettingRoundInfo;
    }


    private void GameManagerPublisher_Deal(object sender, GameManagerPublisher.DealToPlayerEvent args)
    {
        //if (args.player == this.playerName)
        //{
        //    //recive the card
        //}
    }

    private void GameManagerPublisher_BettingRoundInfo(object sender, GameManagerPublisher.SendBettingRoundInfoEvent args)
    {
        //if (args.player == this.playerName)
        //{
        //    //Recive round info and let this player play their hand
        //}
    }


    //Unsubscribe to events
    public void OnDisable()
    {
        GameManagerPublisher.Deal -= GameManagerPublisher_Deal;
    }
}
