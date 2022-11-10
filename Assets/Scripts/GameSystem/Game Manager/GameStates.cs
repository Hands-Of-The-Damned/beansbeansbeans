
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameStates : MonoBehaviour
{

    enum states
    {
        Initial,
        Deal,
        PromptPlayer,
        WaitForPlayer,
        Showdown,
        EndGame
    }

    states state = states.Initial;
    List<PlayerContainer> players;

    int playerOrderIndex;
    List<int> playerOrder;

    public Deck deck;
    bool allPlayersBigBlind = false;
    int bigBlind;
    int smallBlind;
    int round;
    public int pot;
    public int bet;
    int smallBlindAmt;
    int bigBlindAmt;



    public struct PlayerContainer{
        public Player player;
        public bool inRound;
        public bool inGame;
        public bool playedCurrentRound;
        public bool hasBeenBigBlind;
        public bool hasBeenSmallBlind;
        public bool isBigBlind;
        public bool isSmallBlind;
        public int currency;

    }

    void Start()
    {

        bigBlind = 0;
        smallBlind = players.Count -1;
        round = 1;
        pot = 0;
        bet = 0;
        smallBlindAmt = 1;
        bigBlindAmt = 2;
        playerOrderIndex = 0;
        checkGameState();
    }

    public void OnEnable()
    {
    //Subscribe to events
        Player.RoundInfo += Player_RoundInfo;

    }

    public void OnDisable()
    {
        //Unsubscribe to events
        Player.RoundInfo += Player_RoundInfo;
    }

    void Update()
    {
        checkGameState();
    }

    #region Functions
    /// <summary>
    /// This function is called to check if the game state is changed
    /// </summary>
    public void checkGameState()
    {
        switch (state)
        {
            case states.Initial:
                break;

            case states.Deal:
                initialDeal();
                break;

            case states.PromptPlayer:
                promptPlayer();
                break;

            case states.WaitForPlayer:
                break;

            case states.Showdown:
                showDown();
                break;

            case states.EndGame:
                endGame();
                break;

            default:
                break;
        }
    }

    
    /// <summary>
    /// This function will take a player list of the players in the game.
    /// </summary>
    /// <param name="newPlayers"></param>
    //might need to change return type
    public void gameInitialize(List<Player> newPlayers)
    {
        foreach(Player x in newPlayers)
        {
            PlayerContainer player;
            player.player = x;
            player.inGame = true;
            player.inRound = true;
            player.playedCurrentRound = false;
            player.hasBeenBigBlind = false;
            player.hasBeenSmallBlind = false;
            player.isBigBlind = false;
            player.isSmallBlind = false;
            player.currency = x.currency;
        }
        state = states.Deal;
    }

    /// <summary>
    /// Prompt next player to play their hand
    /// </summary>
    public void promptPlayer()
    {
        if(round > 3)
        {
            state = states.Showdown;
        }
        SendRoundInfo(players[playerOrder[playerOrderIndex]].player, bet, pot, round);
        incrementPlayerOrder();
        state = states.WaitForPlayer;
    }

    /// <summary>
    /// Use hand regocnition to declare a winner, move big and small blind, increase round count, set state back to GameLoop
    /// </summary>
    public void showDown()
    {
        //evaluate hands and declare a winner for the round, maybe use an event for this
        state = states.WaitForPlayer;
    }

    /// <summary>
    /// Display the end of game screen
    /// </summary>
    public void endGame()
    {

        // Exit the game(return to menu or open world)
        // maybe some other stuff

    }

    /// <summary>
    /// Deal @param numCards cards to @param player
    /// </summary>
    /// <param name="numCards"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public Card dealToPlayer(int numCards, Player player)
    {
        return deck.deal(numCards)[0];
    }

    /// <summary>
    /// Remove @param player form the current round
    /// </summary>
    /// <param name="player"></param>
    public void removePlayerFormRound(Player player)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].player == player)
            {
                players[i].inRound.Equals(false);
            }
        }
    }

    /// <summary>
    /// Removes the player from the game
    /// </summary>
    /// <param name="player"></param>
    public void removePlayerFromGame(Player player)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].player == player)
            {
                players[i].inGame.Equals(false);
            }
        }
    }

    /// <summary>
    /// Deal the first five cards to the players 1 at a time
    /// </summary>
    /// <param name="players"></param>
    public void initialDeal()
    {
        for (int i = 0; i <= 5; i++)
        {
            foreach (PlayerContainer x in players)
            {
                if(x.inRound)
                {
                    DealToPlayer(x.player, dealToPlayer(1, x.player));
                }
            }
        }
        takeInitalBets();
        state = states.PromptPlayer;
    }

    /// <summary>
    /// Reset the play order list
    /// </summary>
    /// <param name="player"></param>
    public void resetPlayOrder()
    {
        int j = playerOrder[playerOrderIndex];
        playerOrder.Clear();
        for(int i = j+1; i < players.Count; i++)
        {
            if (players[i].inRound)
            {
                playerOrder.Add(i);
            }
        }
        for(int i = 0; i < j; i++)
        {
            if (players[i].inRound)
            {
                playerOrder.Add(i);
            }
        }
        playerOrderIndex = 0;
    }

    /// <summary>
    /// Increments the playerOrder int
    /// checks if there is one player left
    /// </summary>
    public void incrementPlayerOrder()
    {
        int j = 0;
        foreach(PlayerContainer x in players)
        {
            if (x.inRound)
            {
                j++;
            }
        }

        if(j == 1)
        {
            state = states.Showdown;
        }

        if(playerOrderIndex == j - 1)
        {
            state = states.Showdown;
        }
        else
        {
            playerOrderIndex++;
        }
    }

    /// <summary>
    /// Sets the playOrder list
    /// </summary>
    public void setPlayerOrder()
    {
        int i = 0;
        foreach (PlayerContainer x in players)
        {
            if (x.inRound == true)
            {
                playerOrder.Add(i);
            }
            i++;
        }
    }

    /// <summary>
    /// Update the big blind to the next player in the game
    /// </summary>
    /// <returns></returns>
    public void updateBigBlind()
    {
        foreach(PlayerContainer x in players)
        {
            if(x.hasBeenBigBlind == false && x.inGame == true)
            {
                x.hasBeenBigBlind.Equals(true);
                x.isBigBlind.Equals(true);
                break;
            }
            x.isBigBlind.Equals(false);
        }
    }

    /// <summary>
    /// Update the small blind
    /// </summary>
    /// <param name="newSmallBlind"></param>
    public void updateSmallBlind(int newSmallBlind)
    {
        foreach (PlayerContainer x in players)
        {
            if (x.hasBeenSmallBlind == false && x.inGame == true)
            {
                x.hasBeenSmallBlind.Equals(true);
                x.isSmallBlind.Equals(true);
                break;
            }
            x.isBigBlind.Equals(false);
        }
    }
    

    /// <summary>
    /// Set small blind at the begining of the game
    /// </summary>
    public void setSmallBlind()
    {
        players[players.Count - 1].isSmallBlind.Equals(true);
        players[players.Count - 1].hasBeenSmallBlind.Equals(true);
    }

    /// <summary>
    /// Send an event to the big and small blind to take their initial bets
    /// </summary>
    public void takeInitalBets()
    {
        foreach(PlayerContainer x in players)
        {
            if (x.inRound && x.isBigBlind && x.currency - bigBlindAmt > 0)
            {
                pot += bigBlindAmt;
                x.currency.Equals(x.currency - bigBlindAmt);
                TakeBigBet(x.player, bigBlindAmt);
            }
            if (x.inRound && x.isSmallBlind && x.currency - smallBlindAmt > 0)
            {
                pot += smallBlindAmt;
                x.currency.Equals(x.currency - smallBlindAmt);
                TakeSmallBet(x.player, smallBlindAmt);
            }
        }
    }

    /// <summary>
    /// Checks if every player has been the big blind
    /// </summary>
    /// <returns></returns>
    public bool checkAllPlayersBigBlind()
    {
        foreach(PlayerContainer x in players)
        {
            if (!x.hasBeenBigBlind)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Send event to players that there there is going to be a new hand
    /// </summary>
    public void newHand()
    {
         ClearHand();
    }

    #endregion

    /*************************************************************************
     * EVENTS
     *************************************************************************/
    #region Events
    /* EVENT 1
     * Betting Round Event
     * send current pot ammount, current bet ammount, current betting round, string of current players turn
     */


    public class SendBettingRoundInfoEvent
    {
        GameStates eventSystem;
        public Player player;
        public int currentBet;
        public int currentPot;
        public int currentRound;

        public SendBettingRoundInfoEvent(Player playersTurn, int bet, int pot, int round)
        {
            player = playersTurn;
            currentBet = bet;
            currentPot = pot;
            currentRound = round;
        }
    }

    public static event System.EventHandler<SendBettingRoundInfoEvent> BettingRoundInfo;

    public void SendRoundInfo(Player player, int bet, int pot, int round)
    {
        BettingRoundInfo?.Invoke(this, new SendBettingRoundInfoEvent(player, bet, pot, round));
    }


    /* EVENT 2
     * Send Delt Card Event
     * Send delt card to a player
     */

    public class DealToPlayerEvent
    {
        public GameStates eventSystem;
        public Player player;
        public Card card;

        public DealToPlayerEvent(Player playerDeltTo, Card newCard)
        {
            player = playerDeltTo;
            card = newCard;
        }
    }


    public static event System.EventHandler<DealToPlayerEvent> Deal;

    public void DealToPlayer(Player player, Card newCard)
    {
        Deal?.Invoke(this, new DealToPlayerEvent(player, newCard));
    }



    /* EVENT 3
    * Initial Bet: Big Blind 
    * Take bet from big blind
    */

    public class BigBlindBetEvent
    {
        public GameStates eventSystem;
        public int bet;
        public Player player;

        public BigBlindBetEvent(Player bigBlind, int betAmount)
        {
            player = bigBlind;
            bet = betAmount;
        }
    }

    public static event System.EventHandler<BigBlindBetEvent> BigBlind;

    public void TakeBigBet(Player player, int bet)
    {
        BigBlind?.Invoke(this, new BigBlindBetEvent(player, bet));
    }



    /* EVENT 4
    * Initial Bet: Small Blind 
    * Take bet from small blind
    */

    public class SmallBlindBetEvent
    {
        public GameStates eventSystem;
        public int bet;
        public Player player;

        public SmallBlindBetEvent(Player smallBlind, int betAmount)
        {
            player = smallBlind;
            bet = betAmount;
        }
    }

    public static event System.EventHandler<SmallBlindBetEvent> SmallBlind;

    public void TakeSmallBet(Player player, int bet)
    {
        SmallBlind?.Invoke(this, new SmallBlindBetEvent(player, bet));
    }

    /* EVENT 5
    * Clear the players hand for next round
    */

    public class NewHandEvent
    {
        public GameStates eventSystem;


        public NewHandEvent()
        {
        }
    }

    public static event System.EventHandler<NewHandEvent> NewHand;

    public void ClearHand()
    {
        NewHand?.Invoke(this, new NewHandEvent());
    }

    /* EVENT SomeNumber
     * General Major Arcana
     * Probably different events
     * Tell players and GUI what major arcana was played and its effects, send effects to players and GUI
     */



    /*________________Event Handles__________________*/




    /* EVENT HANDLE
     * Major Arcana Effects
     * receive effects: force player to fold for a round, deal cards to a player, force a bet increase
     * There will be others
     * May have to be separate events
     */

    /* EVENT HANDLE    
    * Game Start Event
    * receive list of the players in the game to initialize the game
    */

    //private void StartGame(object sender,)

    /* EVENT HANDLE    
   * Player Betting Round Event
   * receive bet amount, if player folded, if player played major arcana
   */

    private void Player_RoundInfo(object sender, Player.SendRoundDecision args)
    {
        if (args.fold)
        {
            //remove player from queue
            removePlayerFormRound(args.thisPlayer);
        }
        else if (args.raise)
        {
            //player raised
            bet = args.bet;
            pot += args.bet;
            resetPlayOrder();

        }
        else //Player checked the bet
        {
            pot += args.bet;
        }
        state = states.PromptPlayer;
    }



   
    #endregion Events
}


