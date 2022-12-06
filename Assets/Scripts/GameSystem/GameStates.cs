
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
    List<PlayerContainer> players = new List<PlayerContainer>();

    int playerOrderIndex;
    List<int> playerOrder = new List<int>();

    public Deck deck;
    public handRecognition handReco;
    public PlayerInfo info;
//test data
    List<GameObject> newPlayers = new List<GameObject>();
    public GameObject player;
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;
    public GameObject npc4;

    bool allPlayersBigBlind = false;
    int round;
    public int pot;
    public int bet;
    int smallBlindAmt;
    int bigBlindAmt;



    public class PlayerContainer{
        public GameObject player;
        public bool inRound;
        public bool inGame;
        public bool playedCurrentRound;
        public bool hasBeenBigBlind;
        public bool hasBeenSmallBlind;
        public bool isBigBlind;
        public bool isSmallBlind;
        public int currency;
        public int[] handRank;

        public void setHasBeenBigBlind(bool b)
        {
            hasBeenBigBlind = b;
        }

        public void setHasBeenSmallBlind(bool b)
        {
            hasBeenSmallBlind = b;
        }

        public void setIsBigBlind(bool b)
        {
            isBigBlind = b;
        }

        public void setIsSmallBlind(bool b)
        {
            isBigBlind = b;
        }

        public void setHandRank(int[] i)
        {
            handRank = i;
        }
    }

    void Start()
    {
        //test data


        newPlayers.Add(player);
        newPlayers.Add(npc1);
        newPlayers.Add(npc2);
        newPlayers.Add(npc3);
        newPlayers.Add(npc4);

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
        PlayerControllerPoker.RoundInfo += Player_RoundInfo;
        PlayerControllerPoker.ShowDown += Player_ShowDownResponse;
        //PlayerInfo.StartGame += TempGameStart_StartGame;

    }

    public void OnDisable()
    {
        //Unsubscribe to events
        PlayerControllerPoker.RoundInfo -= Player_RoundInfo;
        PlayerControllerPoker.ShowDown -= Player_ShowDownResponse;
        //PlayerInfo.StartGame -= TempGameStart_StartGame;
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
                gameInitialize(newPlayers);
                break;

            case states.Deal:
                Debug.Log("Gamestate: deal");
                
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
    public void gameInitialize(List<GameObject> newPlayers)
    {
        foreach(GameObject x in newPlayers)
        {
            PlayerContainer player = new PlayerContainer();
            player.player = x.gameObject;
            player.inGame = true;
            player.inRound = true;
            player.playedCurrentRound = false;
            player.hasBeenBigBlind = false;
            player.hasBeenSmallBlind = false;
            player.isBigBlind = false;
            player.isSmallBlind = false;
            player.currency = x.GetComponent<PlayerControllerPoker>().currency;
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
        deck.generateDeck();
        deck.shuffle();
        for (int i = 0; i < 5; i++)
        {
            foreach (PlayerContainer x in players)
            {
                if (x.inRound)
                {
                    DealToPlayer(x.player, dealToPlayer(1));
                }
            }
        }
        allPlayersBigBlind = checkAllPlayersBigBlind();
        if (allPlayersBigBlind)
        {
            state = states.EndGame;
            return;
        }
        updateBigBlind();
        takeInitalBets();
        setPlayerOrder();
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
    }
    
    /// <summary>
    /// Use hand regocnition to declare a winner, move big and small blind, increase round count, set state back to GameLoop
    /// </summary>
    public void showDown()
    {
        //evaluate hands and declare a winner for the round, maybe use an event for this
        GameObject winner = compareHands();
        Debug.Log("winner" + winner.GetComponent<PlayerControllerPoker>().playerName);
        resetPlayersInRound();
        resetPlayedCurrentRound();
        setPlayerOrder();
        
        round = 1;
        NewShowDown();
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
    public GameObject compareHands()
    {
        List<GameObject> pokerHandHigh = new List<GameObject>() ;
        List<GameObject> highCard = new List<GameObject>();
        List<GameObject> suitHigh = new List<GameObject>();
        int pokerHand = 0;
        int high = 0;
        int suit = 0;

        //Process hands
        foreach (PlayerContainer x in players)
        {
            if (x.inRound.Equals(true))
            {
                x.setHandRank(handReco.HandRecognition(x.player.GetComponent<PlayerControllerPoker>().hand));
                if (x.handRank[0] > pokerHand)
                {

                    pokerHand = x.handRank[0];
                }
            }
        }

        //find player(s) with the best poker hand
        foreach (PlayerContainer x in players)
        {
            if (x.inRound.Equals(true))
            {
                Debug.Log("good");
                //////////////////////////////////Error
                if(x.handRank[0] == pokerHand)
                {
                    Debug.Log("good");

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
                if (x.inRound.Equals(true) && x.handRank[0] == pokerHand)
                {
                    if (x.handRank[1] > high)
                    {
                        high = x.handRank[1];
                    }
                }
            }
            foreach (PlayerContainer x in players)
            {
                if (x.inRound.Equals(true) && x.handRank[0]==pokerHand)
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
                if (x.inRound.Equals(true) && x.handRank[0] == pokerHand && x.handRank[1] == high)
                {
                    if (x.handRank[2] > suit)
                    {
                        suit = x.handRank[2];
                    }
                }
            }
            foreach (PlayerContainer x in players)
            {
                if (x.inRound.Equals(true) && x.handRank[0] == pokerHand && x.handRank[1] == high)
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
                x.inRound = true;
            }
        }
    }


    /// <summary>
    /// Remove @param player form the current round
    /// </summary>
    /// <param name="player"></param>
    public void removePlayerFormRound(GameObject player)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].player == player)
            {
                players[i].inRound = false;
            }
        }
    }

    /// <summary>
    /// Removes the player from the game
    /// </summary>
    /// <param name="player"></param>
    public void removePlayerFromGame(GameObject player)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].player == player)
            {
                players[i].inGame =false;
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
                x.playedCurrentRound = false;
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
        foreach(int x in playerOrder)
        {
            Debug.Log(x);
        }
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
        playerOrder.Clear();
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
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].hasBeenBigBlind.Equals(false) && players[i].inGame.Equals(true) && players[i].inRound.Equals(true))
            {
                players[i].hasBeenBigBlind = true;
                players[i].setIsBigBlind(true);
                Debug.Log(players[i].player.GetComponent<PlayerControllerPoker>().playerName + " is big blind");
                Debug.Log(players[i].isBigBlind);
                Debug.Log(players[i].hasBeenBigBlind);
                break;
            }
            players[i].setIsBigBlind(false);
        }
    }

    /// <summary>
    /// Update the small blind
    /// </summary>
    public void updateSmallBlind()
    {
        foreach (PlayerContainer x in players)
        {
            if (x.hasBeenSmallBlind.Equals(false) && x.inGame.Equals(true) && x.inRound.Equals(true))
            {
                x.setHasBeenSmallBlind(true);
                x.setIsSmallBlind(true);
                Debug.Log(x.player.GetComponent<PlayerControllerPoker>().playerName + "is small blind");
                break;
            }
            x.setIsSmallBlind(false);
        }
    }
    

    /// <summary>
    /// Set small blind at the begining of the game
    /// </summary>
    public void setSmallBlind()
    {
        players[players.Count - 1].isSmallBlind = true;
        players[players.Count - 1].hasBeenSmallBlind = true;
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
                x.currency = (x.currency - bigBlindAmt);
                TakeBigBet(x.player, bigBlindAmt);
            }
            if (x.inRound && x.isSmallBlind && x.currency - smallBlindAmt > 0)
            {
                pot += smallBlindAmt;
                x.currency = (x.currency - smallBlindAmt);
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
            if (x.hasBeenBigBlind.Equals(false))
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
        public GameObject player;
        public int currentBet;
        public int currentPot;
        public int currentRound;

        public SendBettingRoundInfoEvent(GameObject playersTurn, int bet, int pot, int round)
        {
            player = playersTurn;
            currentBet = bet;
            currentPot = pot;
            currentRound = round;
        }
    }

    public static event System.EventHandler<SendBettingRoundInfoEvent> BettingRoundInfo;

    public void SendRoundInfo(GameObject player, int bet, int pot, int round)
    {
        BettingRoundInfo?.Invoke(this, new SendBettingRoundInfoEvent(player, bet, pot, round));
    }


    /* EVENT 2
     * Send Delt Card Event
     * Send delt card to a player
     */

    public class DealToPlayerEvent
    {
        public GameObject player;
        public Card card;

        public DealToPlayerEvent(GameObject playerDeltTo, Card newCard)
        {
            player = playerDeltTo;
            card = newCard;
        }
    }


    public static event System.EventHandler<DealToPlayerEvent> Deal;

    public void DealToPlayer(GameObject player, Card newCard)
    {
        Deal?.Invoke(this, new DealToPlayerEvent(player, newCard));
    }



    /* EVENT 3
    * Initial Bet: Big Blind 
    * Take bet from big blind
    */

    public class BigBlindBetEvent
    {
        public int bet;
        public GameObject player;

        public BigBlindBetEvent(GameObject bigBlind, int betAmount)
        {
            player = bigBlind;
            bet = betAmount;
        }
    }

    public static event System.EventHandler<BigBlindBetEvent> BigBlind;

    public void TakeBigBet(GameObject player, int bet)
    {
        BigBlind?.Invoke(this, new BigBlindBetEvent(player, bet));
    }



    /* EVENT 4
    * Initial Bet: Small Blind 
    * Take bet from small blind
    */

    public class SmallBlindBetEvent
    {
        public int bet;
        public GameObject player;

        public SmallBlindBetEvent(GameObject smallBlind, int betAmount)
        {
            player = smallBlind;
            bet = betAmount;
        }
    }

    public static event System.EventHandler<SmallBlindBetEvent> SmallBlind;

    public void TakeSmallBet(GameObject player, int bet)
    {
        SmallBlind?.Invoke(this, new SmallBlindBetEvent(player, bet));
    }

    /* EVENT 5
    * Clear the players hand for next round
    */

    public class NewHandEvent
    {


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

    //private void TempGameStart_StartGame(object sender, PlayerInfo.StartGameEvent args)
    //{
    //    gameInitialize(args.players);
    //}

    /* EVENT HANDLE    
   * Player Betting Round Event
   * receive bet amount, if player folded, if player played major arcana
   */

    private void Player_RoundInfo(object sender, PlayerControllerPoker.SendRoundDecision args)
    {
        if (args.fold)
        {
            //remove player from queue
            removePlayerFormRound(args.thisPlayer);
            incrementPlayerOrder();
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
            incrementPlayerOrder();
        }
        state = states.PromptPlayer;
    }

    /* EVENT HANDLE    
   * Showdown Response
   * continue to the next hand
   */

    private void Player_ShowDownResponse(object sender, PlayerControllerPoker.ShowDownResponse args)
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