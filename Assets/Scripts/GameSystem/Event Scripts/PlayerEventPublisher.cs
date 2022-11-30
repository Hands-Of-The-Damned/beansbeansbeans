using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventPublisher : MonoBehaviour
{
    /* EVENT 1
     * Send Betting Round Decision
     * Send player bet ammount, player fold bool, bool raise
     */

    public class SendRoundDecision
    {
        public PlayerEventPublisher eventSystem;
        public bool fold;
        public bool raise;
        public int bet;

        public SendRoundDecision(bool didFold, bool willRaise,  int betAmt)
        {
            fold = didFold;
            raise = willRaise;
            bet = betAmt;
        }
    }

    public static event System.EventHandler<SendRoundDecision> RoundInfo;

    public void SendRoundInfo(bool didFold, bool isRaise, int betAmt)
    {
        RoundInfo?.Invoke(this, new SendRoundDecision(didFold, isRaise, betAmt));
    }


    /* EVENT 2
     * Play major arcana
     * play some major arcana card
     */

}
