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

    public string file;
    GameObject PlayerPreFab;
    public playerHand hand;
    public string playerName;
    public int round;
    public int currentBetToMatch;
    public int currentPot;
    public int bet;
    public int currency;
    public bool playedArcanaThisRound;
    public bool raised;
    public bool folded;
    public bool isAI;
    public bool isTurn;
    states state;


    private void Awake()
    {
        this.Ask(file);
    }
    private void Start()
    {
        state = states.Waiting;
        round = 0;
        currentBetToMatch = 0;
        currentPot = 0;
        playedArcanaThisRound = false;
        raised = false;
        folded = false;
        isTurn = false;
        //playerName = "Brongus";
    }

    private void Update()
    {
        checkState();
        if (Input.GetKeyDown(KeyCode.F) && isTurn)
        {
            setFold(!folded);
            Debug.Log(playerName + " fold " + folded);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && isTurn)
        {
            setBet(bet += 1);
            Debug.Log(playerName + " bet " + bet);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && isTurn)
        {
            setBet(bet -= 1);
            Debug.Log(playerName + " bet " + bet);
        }
        if (Input.GetKeyDown(KeyCode.C) && isTurn)
        {
            setBet(currentBetToMatch);
            Debug.Log(playerName + " bet "+bet);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isTurn)
        {
            playHand();
            Debug.Log(playerName + " ended turn");
        }
    }

    public void OnEnable()
    {
        GameStates.Deal += GameStates_Deal;
        GameStates.BettingRoundInfo += GameStates_BettingRoundInfo;
        GameStates.BigBlind += GameStates_BigBlindBetEvent;
        GameStates.SmallBlind += GameStates_SmallBlindBetEvent;
        GameStates.NewHand += GameStates_NewHand;
        PlayerInfo.SendPlayer += PlayerInfo_PlayerInfo;
    }

    public void OnDisable()
    {
        GameStates.Deal -= GameStates_Deal;
        GameStates.BettingRoundInfo -= GameStates_BettingRoundInfo;
        GameStates.BigBlind -= GameStates_BigBlindBetEvent;
        GameStates.SmallBlind -= GameStates_SmallBlindBetEvent;
        GameStates.NewHand -= GameStates_NewHand;
        PlayerInfo.SendPlayer -= PlayerInfo_PlayerInfo;
    }




    #region Functions

    public Player()
    {
        this.Ask(file);
    }


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
        Debug.Log(playerName + " turn");
        Debug.Log(hand.hand.ToString());
        state = states.Waiting;
    }


    #region Helperfunctions

    public void setName(string name)
    {
        playerName = name;
    }

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
        if(bet > currentBetToMatch)
        {
            raised = true;
        }
        else
        {
            raised = false;
        }
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
            this.SendRoundInfo(this, folded, raised, bet);
        }
        else if (raised)
        {
            this.SendRoundInfo(this, folded, raised, bet);
        }
        else
        {
            this.SendRoundInfo(this, folded, raised, bet);
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

            player.setFold(false);
            player.setBet(0);
            player.setRaised(false);
            player.isTurn = false;

        }
    }

    public static event System.EventHandler<SendRoundDecision> RoundInfo;

    public void SendRoundInfo(Player player, bool didFold, bool isRaise, int betAmt)
    {
        RoundInfo?.Invoke(this, new SendRoundDecision(player, didFold, isRaise, betAmt));
    }


     /* EVENT 2
     * Showdown Response
     * Respond to showdown
     */

    public class ShowDownResponse
    {
        public ShowDownResponse()
        {

        }
    }

    public static event System.EventHandler<ShowDownResponse> ShowDown;

    public void SendShowDownResponse()
    {
        ShowDown?.Invoke(this, new ShowDownResponse());
    }


    /* EVENT 3
     * Ask for Player Info
     * Ask the data base for this players info
     */

    public class AskForInfo
    {
        public string file;
        public AskForInfo(string playerFile)
        {
            file = playerFile;
        }
    }

    public static event System.EventHandler<AskForInfo> AskInfo;

    public void Ask(string playerFile)
    {
        AskInfo?.Invoke(this, new AskForInfo(playerFile));
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
            isTurn = true;
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

    public void PlayerInfo_PlayerInfo(object sender, PlayerInfo.SendPlayerInfoEvent args)
    {
        if(args.player == this)
        {
            setName(args.playerName);
            setAI(args.playerIsAI);
            setCurrency(args.playerCurrency);
        }
    }


    #endregion
}
