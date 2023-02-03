using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerPoker : MonoBehaviour
{

    enum States
    {
        Waiting,
        PlayHand
    }

    #region Properties

    public string file;
    public playerHand hand;

    public string PlayerName { get; set; }
    public int CurrentBetToMatch { get; set; } = 0;
    public int CurrentPot { get; set; } = 0;
    public bool PlayedArcanaThisRound { get; set; } = false;
    public bool Raised { get; set; } = false;
    public bool Folded { get; set; } = false;
    public bool IsAI { get;  set; }
    public bool IsTurn { get; set; } = false;
    public bool IsShowdown { get; set; } = false;
    public PokerGameData PokerGameData { get; set; }

    States state = States.Waiting;

    public int Currency
    {
        get => Currency;
        set
        {
            if (Currency - value > 0)
            {
                Currency -= value;
            }
        }
    }

    public int Round
    {
        get => Round;
        set
        {
            if (value != Round)
            {
                PlayedArcanaThisRound = false;
            }
            Round = value;
        }
    }

    public int Bet
    {
        get => Bet;
        set
        {
            if (Bet == 0 && value < 0)
            {
                Bet = 0;
            }
            else
            {
                Bet = value;
            }
            if (Bet > CurrentBetToMatch)
            {
                Raised = true;
            }
            else
            {
                Raised = false;
            }
        }
    }

    #endregion

    private void Update()
    {
        CheckState();
        if (Input.GetKeyDown(KeyCode.F) && IsTurn)
        {
            Folded = !Folded;
            Debug.Log(PlayerName + " fold " + Folded);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsTurn)
        {
            Bet += 1;
            Debug.Log(PlayerName + " bet " + Bet);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && IsTurn)
        {
            Bet -= 1;
            Debug.Log(PlayerName + " bet " + Bet);
        }
        if (Input.GetKeyDown(KeyCode.C) && IsTurn)
        {
            Bet = CurrentBetToMatch;
            Debug.Log(PlayerName + " bet "+Bet);
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsTurn)
        {
            PlayHand();
            Debug.Log(PlayerName + " ended turn");
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsShowdown)
        {
            IsShowdown = false;
            SendShowDownResponse();
        }
    }

    public void OnEnable()
    {
        GameStates.Deal += GameStates_Deal;
        GameStates.BettingRoundInfo += GameStates_BettingRoundInfo;
        GameStates.BigBlind += GameStates_BigBlindBetEvent;
        GameStates.SmallBlind += GameStates_SmallBlindBetEvent;
        GameStates.NewHandEvent += GameStates_NewHand;
        PlayerInfo.SendPlayer += PlayerInfo_PlayerInfo;
        GameStates.ShowDownEvents += GameStates_Showdown;
    }

    public void OnDisable()
    {
        GameStates.Deal -= GameStates_Deal;
        GameStates.BettingRoundInfo -= GameStates_BettingRoundInfo;
        GameStates.BigBlind -= GameStates_BigBlindBetEvent;
        GameStates.SmallBlind -= GameStates_SmallBlindBetEvent;
        GameStates.NewHandEvent -= GameStates_NewHand;
        PlayerInfo.SendPlayer -= PlayerInfo_PlayerInfo;
        GameStates.ShowDownEvents += GameStates_Showdown;

    }


    #region Functions

    public PlayerControllerPoker()
    {
       
    }


    public void CheckState()
    {
        switch (state)
        {
            case States.Waiting:
                break;

            case States.PlayHand:
                PlayerTurn();
                break;

            default:
                break;

        }
    }

   /// <summary>
   /// Call UI to enable buttons and let player conduct their turn
   /// </summary>
    public void PlayerTurn()
    {
        //send event to UI
        Debug.Log(PlayerName + " turn");
        foreach(Card x in hand.hand)
        {
        Debug.Log(x.CardName);
        }
        Debug.Log("bet to match: "+CurrentBetToMatch);
        Debug.Log("pot: "+ CurrentPot);
        Debug.Log("round: "+ Round);
        Debug.Log("currency: "+Currency);
        state = States.Waiting;
    }

    /// <summary>
    /// Reply to the Game with round decision
    /// </summary>
    public void PlayHand()
    {
        SendRoundInfo(this, Folded, Raised, Bet);
    }
    #endregion


    /********************************************************************
     *                              EVENTS
     ********************************************************************/


    #region Events
    /* EVENT 1
     * Send Betting Round Decision
     * Send player bet ammount, player fold bool, bool raise
     */

    public class RoundDecisionEventArgs
    {
        public bool fold;
        public bool raise;
        public int bet;
        public PlayerControllerPoker thisPlayer;

        public RoundDecisionEventArgs(PlayerControllerPoker player, bool didFold, bool willRaise, int betAmt)
        {
            thisPlayer = player;
            fold = didFold;
            raise = willRaise;
            bet = betAmt;

            player.Folded = fold;
            player.Currency -= bet;
            player.Raised = false;
            player.IsTurn = false;

        }
    }

    public static event System.EventHandler<RoundDecisionEventArgs> RoundInfo;

    public void SendRoundInfo(PlayerControllerPoker player, bool didFold, bool isRaise, int betAmt)
    {
        RoundInfo?.Invoke(this, new RoundDecisionEventArgs(player, didFold, isRaise, betAmt));
    }


     /* EVENT 2
     * Showdown Response
     * Respond to showdown
     */

    public class ShowDownResponseEventArgs
    {
        public ShowDownResponseEventArgs()
        {

        }
    }

    public static event System.EventHandler<ShowDownResponseEventArgs> ShowDown;

    public void SendShowDownResponse()
    {
        ShowDown?.Invoke(this, new ShowDownResponseEventArgs());
    }


    /* EVENT 3
     * Ask for Player Info
     * Ask the data base for this players info
     */

    public class AskForInfoEventArgs
    {
        public string file;
        public GameObject player;
        public AskForInfoEventArgs(GameObject thisPlayer, string playerFile)
        {
            file = playerFile;
            player = thisPlayer;
        }
    }

    public static event System.EventHandler<AskForInfoEventArgs> AskInfo;

    public void Ask(GameObject player, string playerFile)
    {
        AskInfo?.Invoke(this, new AskForInfoEventArgs(player, playerFile));
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
        if (args.player == gameObject)
        {
            //recive the card
            hand.hand.Add(args.card);
        }
    }

    private void GameStates_BettingRoundInfo(object sender, GameStates.SendBettingRoundInfoEvent args)
    {
        if (args.player == gameObject)
        {
            //Recive round info and let this player play their hand
            CurrentBetToMatch = args.currentBet;
            CurrentPot = args.currentPot;
            Round = (args.currentRound);
            IsTurn = true;
            state = States.PlayHand;
        }
    }


    public void GameStates_BigBlindBetEvent(object sender, GameStates.BigBlindBetEvent args)
    {
        if (args.player == gameObject)
        {
            Bet = (args.bet);
        }
    }

    public void GameStates_SmallBlindBetEvent(object sender, GameStates.SmallBlindBetEvent args)
    {
        if (args.player == gameObject)
        {
            Bet = (args.bet);
        }
    }

    public void GameStates_NewHand(object sender, GameStates.NewHandEventArgs args)
    {
        hand.hand.Clear();
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

    public void PlayerInfo_PlayerInfo(object sender, PlayerInfo.SendPlayerInfoEvent args)
    {
        if(args.player == gameObject)
        {
            name = args.playerName;
            IsAI = args.playerIsAI;
            Currency = args.playerCurrency;
        }
    }

    public void GameStates_Showdown(object sender, GameStates.ShowDownEventArgs args)
    {
        IsShowdown = true;
    }


    #endregion
}
