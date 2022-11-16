using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    enum states
    {
        Waiting,
        PlayHand
    }

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
    public bool isAI;
    states state; 

    private void Start()
    {
        state = states.Waiting;
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
        checkState();
    }

    public void OnEnable()
    {
        GameStates.Deal += GameStates_Deal;
        GameStates.BettingRoundInfo += GameStates_BettingRoundInfo;
        GameStates.BigBlind += GameStates_BigBlindBetEvent;
        GameStates.SmallBlind += GameStates_SmallBlindBetEvent;
        GameStates.NewHand += GameStates_NewHand;
    }

    public void OnDisable()
    {
        GameStates.Deal -= GameStates_Deal;
        GameStates.BettingRoundInfo -= GameStates_BettingRoundInfo;
        GameStates.BigBlind -= GameStates_BigBlindBetEvent;
        GameStates.SmallBlind -= GameStates_SmallBlindBetEvent;
        GameStates.NewHand -= GameStates_NewHand;

    }




    #region Functions

    public void checkState()
    {
        switch (state)
        {
            case states.Waiting:
                break;

            case states.PlayHand:
                playerTurn();
                break;

            default:
                break;

        }
    }

   /// <summary>
   /// Call UI to enable buttons and let player conduct their turn
   /// </summary>
    public void playerTurn()
    {
        //send event to UI

        state = states.Waiting;
    }


    #region Helperfunctions

    /// <summary>
    /// Set the AI bool using parameter
    /// </summary>
    /// <param name="x"></param>
    public void setAI(bool x)
    {
        isAI = x;
    }


    /// <summary>
    /// Set the folded bool with parameter
    /// </summary>
    /// <param name="x"></param>
    public void setFold(bool x)
    {
        folded = x;
    }

    /// <summary>
    /// Set the bet to an amount using parameter
    /// </summary>
    /// <param name="x"></param>
    public void setBet(int x)
    {
        bet = x;
    }

    /// <summary>
    /// Set the players currency amount using parameter
    /// </summary>
    /// <param name="x"></param>
    public void setCurrency(int x)
    {
        currency = x;
    }

    /// <summary>
    /// Set the raised bool using parameter
    /// </summary>
    /// <param name="x"></param>
    public void setRaised(bool x)
    {
        raised = x;
    }


    /// <summary>
    /// Check if the game and player are in the same round of game play
    /// if not, reset the played arcana bool, updates the round
    /// </summary>
    /// <param name="incomingRound"></param>
    public void checkCurrentRound(int incomingRound)
    {
        if (incomingRound != round)
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
        if (currency - blindAmt > 0)
        {
            currency -= blindAmt;
        }
    }
    #endregion

    #endregion


    /********************************************************************
     *                              EVENTS
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


    public class ShowDownResponse
    {
        public Player eventSystem;
        public ShowDownResponse()
        {

        }
    }

    public static event System.EventHandler<ShowDownResponse> ShowDown;

    public void SendShowDownResponse()
    {
        ShowDown?.Invoke(this, new ShowDownResponse());
    }

    /* EVENT 2
     * Play major arcana
     * play some major arcana card
     */



    /*________________Event Handles__________________*/

    public void GameStats_PlayerStats()
    {
        //recive the players stats and set all relevent variables
    }

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
            state = states.PlayHand;
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

    public void GameStates_NewHand(object sender, GameStates.NewHandEvent args)
    {
        this.hand.hand.Clear();
    }

    public void UI_BetButton()
    {
        //set bet amount
        //check if player raised
    }

    public void UI_FoldButton()
    {
        //set fold bool
    }


    #endregion
}
