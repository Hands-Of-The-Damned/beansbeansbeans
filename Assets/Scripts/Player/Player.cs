using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Player eventSystem;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playHand();
        }
    }

    public void OnEnable()
    {
        GameStates.Deal += GameStates_Deal;
        GameStates.BettingRoundInfo += GameStates_BettingRoundInfo;
        GameStates.BigBlind += GameStates_BigBlindBetEvent;
        GameStates.SmallBlind += GameStates_SmallBlindBetEvent;
        GameStates.NewHand += GameStates_Newhand;
    }

    public void OnDisable()
    {
        GameStates.Deal -= GameStates_Deal;
        GameStates.BettingRoundInfo -= GameStates_BettingRoundInfo;
        GameStates.BigBlind -= GameStates_BigBlindBetEvent;
        GameStates.SmallBlind -= GameStates_SmallBlindBetEvent;
        GameStates.NewHand -= GameStates_Newhand;

    }




    #region Functions
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
    #endregion


    /********************************************************************
     * EVENTS
     ********************************************************************/


    #region Events
    /* EVENT 1
     * Send Betting Round Decision
     * Send player bet ammount, player fold bool, bool raise
     */

    public class SendRoundDecision
    {
        public Player eventSystem;
        public bool fold;
        public bool raise;
        public int bet;
        public Player thisPlayer;

        public SendRoundDecision(Player player, bool didFold, bool willRaise, int betAmt)
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



    /*________________Event Handles__________________*/
    private void GameStates_Deal(object sender, GameStates.DealToPlayerEvent args)
    {
        if (args.player == this)
        {
            //recive the card
            hand.hand.Add(args.card);
        }
    }

    private void GameStates_BettingRoundInfo(object sender, GameStates.SendBettingRoundInfoEvent args)
    {
        if (args.player == this)
        {
            //Recive round info and let this player play their hand
            currentBetToMatch = args.currentBet;
            currentPot = args.currentPot;
            checkCurrentRound(args.currentRound);
        }
    }


    public void GameStates_BigBlindBetEvent(object sender, GameStates.BigBlindBetEvent args)
    {
        if (args.player == this)
        {
            blindBet(args.bet);
        }
    }

    public void GameStates_SmallBlindBetEvent(object sender, GameStates.SmallBlindBetEvent args)
    {
        if (args.player == this)
        {
            blindBet(args.bet);
        }
    }

    public void GameStates_Newhand(object sender)
    {
        this.hand.hand.Clear();
    }

    #endregion
}
