using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerListener : MonoBehaviour
{

    GameStates game;

    //Subscribe to events
    public void OnEnable()
    {
        PlayerEventPublisher.RoundInfo += PlayerEventPublisher_RoundInfo;
    }

    private void PlayerEventPublisher_RoundInfo(object sender, PlayerEventPublisher.SendRoundDecision args)
    {
        if (args.fold)
        {
            //remove player from queue
            game.removePlayerFormRound(args.thisPlayer);
            game.playerReply = true;
        }
        else if (args.raise)
        {
            //player raised
            game.bet = args.bet;
            game.pot += args.bet;
            game.resetQueue(args.thisPlayer);
            game.playerReply = true;
        }
        else //Player checked the bet
        {
            game.pot += args.bet;
            game.playerReply = true;
        }
    }



    //Unsubscribe to events
    public void OnDisable()
    {
        PlayerEventPublisher.RoundInfo += PlayerEventPublisher_RoundInfo;
    }

    
}
