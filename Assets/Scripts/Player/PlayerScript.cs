using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerEventPublisher eventSystem;
    public playerHand hand;
    public int round;
    public int currentBetToMatch;
    public int currentPot;
    public int bet;
    public int currency;
    public bool playedArcanaThisRound;
    public bool raised;
    public bool folded;

    private void Start()
    {
        round = 0;
        currentBetToMatch = 0;
        currentPot = 0;
        playedArcanaThisRound = false;
        raised = false;
        folded = false;
        currency = 0;
    }

    /// <summary>
    /// Check if the game and player are in the same round of game play
    /// if not, reset the played arcana bool, updates the round
    /// </summary>
    /// <param name="incomingRound"></param>
    public void checkCurrentRound(int incomingRound)
    {
        if(incomingRound != round)
        {
            playedArcanaThisRound = false;
        }
        round = incomingRound;
    }

    /// <summary>
    /// Reply to the Game with round decision
    /// </summary>
    public void playHand()
    {
        if (folded)
        {
            eventSystem.SendRoundInfo(this, folded, raised, bet);
        }
        else if (raised)
        {
            eventSystem.SendRoundInfo(this, folded, raised, bet);
        }
        else
        {
            eventSystem.SendRoundInfo(this, folded, raised, bet);
        }
    }

    /// <summary>
    /// Deduct blind bet from players currency
    /// </summary>
    /// <param name="blindAmt"></param>
    public void blindBet(int blindAmt)
    {
        if(currency - blindAmt > 0)
        {
            currency -= blindAmt;
        }
    }
}
