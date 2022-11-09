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
        public Player thisPlayer;

        public SendRoundDecision(Player player ,bool didFold, bool willRaise,  int betAmt)
        {
            thisPlayer = player;
            fold = didFold;
            raise = willRaise;
            bet = betAmt;
        }
    }

    public static event System.EventHandler<SendRoundDecision> RoundInfo;

    public void SendRoundInfo(Player player, bool didFold, bool isRaise, int betAmt)
    {
        RoundInfo?.Invoke(this, new SendRoundDecision(player, didFold, isRaise, betAmt));
    }


    /* EVENT 2
     * Play major arcana
     * play some major arcana card
     */

}
