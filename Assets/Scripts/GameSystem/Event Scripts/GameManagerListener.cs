using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerListener : MonoBehaviour
{

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
        }

       //set current bet to bet amount

        if (args.raise)
        {
            //reset the queue
        }
    }



    //Unsubscribe to events
    public void OnDisable()
    {
        
    }

    
}