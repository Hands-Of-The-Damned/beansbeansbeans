
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameStates : MonoBehaviour
{

    enum states
    {
        Initial,
        Deal,
        DealNextCard,
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
    public handRecognition handReco;
    bool allPlayersBigBlind = false;
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
        public int[] handRank;
    }

    void Start()
    {
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
        Player.ShowDown += Player_ShowDownResponse;
        PlayerInfo.StartGame += TempGameStart_StartGame;

    }

    public void OnDisable()
    {
        //Unsubscribe to events
        Player.RoundInfo -= Player_RoundInfo;
        Player.ShowDown -= Player_ShowDownResponse;
        PlayerInfo.StartGame -= TempGameStart_StartGame;
    }

    void Update()
    {
        checkGameState();
    }

    #region Functions
    #region StateFunctions

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
                Debug.Log("Gamestate: deal");
                if (allPlayersBigBlind)
                {
                    state = states.EndGame;
                }
                initialDeal();
                break;

            case states.DealNextCard:
                Debug.Log("Gamestate: dealnextcard");
                dealNextCard();
                break;

            case states.PromptPlayer:
                Debug.Log("Gamestate: prompt player");
                promptPlayer();
                break;

            case states.WaitForPlayer:
                Debug.Log("Gamestate: wait");
                break;

            case states.Showdown:
                Debug.Log("Gamestate: showdown");
                showDown();
                break;

            case states.EndGame:
                Debug.Log("Gamestate: endgame");
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
            player.handRank = new int[]{0,0,0};
            players.Add(player);
            Debug.Log("player added: " + x);
        }
        setSmallBlind();
        state = states.Deal;
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
                if (x.inRound)
                {
                    DealToPlayer(x.player, dealToPlayer(1));
                    Debug.Log("dealt card to: " + x);
                }
            }
        }
        allPlayersBigBlind = checkAllPlayersBigBlind();
        updateBigBlind();
        takeInitalBets();
        state = states.PromptPlayer;
    }

    /// <summary>
    /// Deals the next card to all players
    /// </summary>
    public void dealNextCard()
    {
        foreach (PlayerContainer x in players)
        {
            if (x.inRound)
            {
                DealToPlayer(x.player, dealToPlayer(1));
            }
        }
        resetPlayedCurrentRound();
        state = states.PromptPlayer;
    }

    /// <summary>
    /// Prompt next player to play their hand
    /// </summary>
    public void promptPlayer()
    {
        checkForShowdown();
        if(round > 3)
        {
            state = states.Showdown;
            updateSmallBlind();
            return;
        }
        if(playerOrderIndex == -1)
        {
            setPlayerOrder();
            state = states.DealNextCard;
            return;
        }
        SendRoundInfo(players[playerOrder[playerOrderIndex]].player, bet, pot, round);
        state = states.WaitForPlayer;
        incrementPlayerOrder();
    }
    
    /// <summary>
    /// Use hand regocnition to declare a winner, move big and small blind, increase round count, set state back to GameLoop
    /// </summary>
    public void showDown()
    {
        //evaluate hands and declare a winner for the round, maybe use an event for this
        Player winner = compareHands();
        Debug.Log(winner.playerName);
        resetPlayersInRound();
        resetPlayedCurrentRound();
        round = 1;
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



    #endregion

    #region HelperFunctions

    /// <summary>
    /// This compares all players hands and returns the winner
    /// </summary>
    /// <returns></returns>
    public Player compareHands()
    {
        List<Player> pokerHandHigh = new List<Player>() ;
        List<Player> highCard = new List<Player>();
        List<Player> suitHigh = new List<Player>();
        int pokerHand = 0;
        int high = 0;
        int suit = 0;

        //Process hands
        foreach (PlayerContainer x in players)
        {
            if (x.inRound)
            {
                x.handRank.Equals(handReco.HandRecognition(x.player.hand));
                if (x.handRank[0] > pokerHand)
                {
                    pokerHand = x.handRank[0];
                }
            }
        }

        //find player(s) with the best poker hand
        foreach (PlayerContainer x in players)
        {
            if (x.inRound)
            {
                if(x.handRank[0] == pokerHand)
                {
                    pokerHandHigh.Add(x.player);
                }
            }
        }

        //check if there is one player with the best hand
        //if not find the players with the best high card
        if (pokerHandHigh.Count != 1)
        {
            foreach (PlayerContainer x in players)
            {
                if (x.inRound && x.handRank[0] == pokerHand)
                {
                    if (x.handRank[1] > high)
                    {
                        high = x.handRank[1];
                    }
                }
            }
            foreach (PlayerContainer x in players)
            {
                if (x.inRound && x.handRank[0]==pokerHand)
                {
                    if (x.handRank[1] == high)
                    {
                        highCard.Add(x.player);
                    }
                }
            }
        }
        else
        {
            // return the player with the best poker hand
            return pokerHandHigh[0];
        }

        //check if there is one player with the best high card
        //if not find the player with the highest suit
        if (highCard.Count != 1)
        {
            foreach (PlayerContainer x in players)
            {
                if (x.inRound && x.handRank[0] == pokerHand && x.handRank[1] == high)
                {
                    if (x.handRank[2] > suit)
                    {
                        suit = x.handRank[2];
                    }
                }
            }
            foreach (PlayerContainer x in players)
            {
                if (x.inRound && x.handRank[0] == pokerHand && x.handRank[1] == high)
                {
                    if (x.handRank[2] == suit)
                    {
                        suitHigh.Add(x.player);
                    }
                }
            }
        }
        else
        {
            //return player with best high card
            return highCard[0];
        }
        //return player with best suit
        return suitHigh[0];

    }

    /// <summary>
    /// Deal @param numCards cards to @param player
    /// </summary>
    /// <param name="numCards"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public Card dealToPlayer(int numCards)
    {
        return deck.deal(numCards)[0];
    }

    /// <summary>
    /// Set inRound bool of all players still in the game to true
    /// </summary>
    public void resetPlayersInRound()
    {
        foreach(PlayerContainer x in players)
        {
            if (x.inGame)
            {
                x.inRound.Equals(true);
            }
        }
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
    /// Reset the playedcurrentround bool for players in round
    /// </summary>
    public void resetPlayedCurrentRound()
    {
        foreach(PlayerContainer x in players)
        {
            if (x.inRound)
            {
                x.playedCurrentRound.Equals(false);
            }
        }
    }

    /// <summary>
    /// Reset the play order list after raise
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
    /// Increments the playerOrder int, if at the end of the player order increment round
    /// </summary>
    public void incrementPlayerOrder()
    {
         playerOrderIndex++;
        if(playerOrderIndex >= playerOrder.Count)
        {
            playerOrderIndex = -1;
            round++;
        }
    }

    /// <summary>
    /// Check if there is one player in the current round
    /// </summary>
    public void checkForShowdown()
    {
        int j = 0;
        foreach (PlayerContainer x in players)
        {
            if (x.inRound)
            {
                j++;
            }
        }

        if (j == 1)
        {
            state = states.Showdown;
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
        playerOrderIndex = 0;
    }

    /// <summary>
    /// Update the big blind to the next player in the game
    /// </summary>
    /// <returns></returns>
    public void updateBigBlind()
    {
        foreach(PlayerContainer x in players)
        {
            if(x.hasBeenBigBlind == false && x.inGame == true && x.inRound == true)
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
    public void updateSmallBlind()
    {
        foreach (PlayerContainer x in players)
        {
            if (x.hasBeenSmallBlind == false && x.inGame == true && x.inRound == true)
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

    #endregion

    /*************************************************************************
     *                             EVENTS
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

    /* EVENT 6
    * Tell players that showdown is happening
    */

    public class ShowDownEvent
    {
        public GameStates eventSystem;

        public ShowDownEvent()
        {

        }
    }

    public static event System.EventHandler<ShowDownEvent> ShowDown;

    public void NewShowDown()
    {
        ShowDown?.Invoke(this, new ShowDownEvent());
    }

    /* EVENT SomeNumber
     * General Major Arcana
     * Probably different events
     * Tell players and GUI what major arcana was played and its effects, send effects to players and GUI
     */



    /*________________Event Handles__________________*/



    /* EVENT HANDLE    
    * Game Start Event
    * receive list of the players in the game to initialize the game
    */

    private void TempGameStart_StartGame(object sender, PlayerInfo.StartGameEvent args)
    {
        gameInitialize(args.players);
    }

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

    /* EVENT HANDLE    
   * Showdown Response
   * continue to the next hand
   */

    private void Player_ShowDownResponse(object sender, Player.ShowDownResponse args)
    {
        newHand();
        pot = 0;
        bet = 0;
        state = states.Deal;

    }

    /* EVENT HANDLE
   * Major Arcana Effects
   * receive effects: force player to fold for a round, deal cards to a player, force a bet increase
   * There will be others
   * May have to be separate events
   */

    #endregion Events
}