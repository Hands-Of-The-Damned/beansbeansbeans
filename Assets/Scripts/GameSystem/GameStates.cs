using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStates : MonoBehaviour
{

    // Game state representations.
    enum States
    {
        Initial,
        Deal,
        DealNextCard,
        PromptPlayer,
        WaitForPlayer,
        Showdown,
        EndGame
    }

    States state = States.Deal;
    public List<PlayerControllerPoker> Players = new();
    public List<int> PlayerOrder = new();


    public Deck deck;
    public handRecognition handReco;
    public PlayerInfo info;

    bool allPlayersBigBlind = false;

    int playerOrderIndex = 0;
    int round = 1;
    public int pot = 0;
    public int bet = 0;
    int smallBlindAmt = 1;
    int bigBlindAmt = 2;

    public void OnEnable()
    {
        //Subscribe to events
        PlayerControllerPoker.RoundInfo += Player_RoundInfo;
        PlayerControllerPoker.ShowDown += Player_ShowDownResponse;
    }

    public void OnDisable()
    {
        //Unsubscribe to events
        PlayerControllerPoker.RoundInfo -= Player_RoundInfo;
        PlayerControllerPoker.ShowDown -= Player_ShowDownResponse;
    }

    void Update()
    {
        CheckGameState();
    }

    #region Functions
    #region StateFunctions

    /// <summary>
    /// This function is called to check if the game state is changed
    /// </summary>
    public void CheckGameState()
    {
        switch (state)
        {
            case States.Deal:
                Debug.Log("Gamestate: deal");
                initialDeal();
                break;

            case States.DealNextCard:
                Debug.Log("Gamestate: dealnextcard");
                dealNextCard();
                break;

            case States.PromptPlayer:
                Debug.Log("Gamestate: prompt player");
                promptPlayer();
                break;

            case States.WaitForPlayer:
                Debug.Log("Gamestate: wait");
                break;

            case States.Showdown:
                Debug.Log("Gamestate: showdown");
                showDown();
                break;

            case States.EndGame:
                Debug.Log("Gamestate: endgame");
                endGame();
                break;

            default:
                break;
        }
    }


    #region Removed Functions

    /*
     *  REASON FOR REMOVAL: GameInitialize
     *  The player container holds per-player per-game specific data.
     *  I believe we could store this data on the player class and pass the players in as a variable to the GameStates script in the inspector.
     */

    /// <summary>
    /// This function will take a player list of the players in the game.
    /// </summary>
    /// <param name="newPlayers"></param>
    //might need to change return type
    //public void GameInitialize(List<GameObject> newPlayers)
    //{
    //    foreach(GameObject x in newPlayers)
    //    {
    //        PlayerContainer player = new();
    //        player.player = x.gameObject;
    //        player.inGame = true;
    //        player.inRound = true;
    //        player.playedCurrentRound = false;
    //        player.hasBeenBigBlind = false;
    //        player.hasBeenSmallBlind = false;
    //        player.isBigBlind = false;
    //        player.isSmallBlind = false;
    //        player.currency = x.GetComponent<PlayerControllerPoker>().currency;
    //        player.handRank = new int[]{0,0,0};
    //        Players.Add(player);
    //        Debug.Log("player added: " + x);
    //    }
    //    setSmallBlind();
    //    state = States.Deal;
    //}

    #endregion

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
            foreach (PlayerContainer x in Players)
            {
                if (x.InRound)
                {
                    DealToPlayer(x.Player, DealToPlayer(1));
                }
            }
        }
        allPlayersBigBlind = CheckAllPlayersBigBlind();
        if (allPlayersBigBlind)
        {
            state = States.EndGame;
            return;
        }
        UpdateBigBlind();
        TakeInitalBets();
        SetPlayerOrder();
        state = States.PromptPlayer;
    }

    /// <summary>
    /// Deals the next card to all players
    /// </summary>
    public void dealNextCard()
    {
        foreach (PlayerContainer x in Players)
        {
            if (x.InRound)
            {
                DealToPlayer(x.Player, DealToPlayer(1));
            }
        }
        ResetPlayedCurrentRound();
        state = States.PromptPlayer;
    }

    /// <summary>
    /// Prompt next player to play their hand
    /// </summary>
    public void promptPlayer()
    {
        CheckForShowdown();
        if(round > 3)
        {
            state = States.Showdown;
            UpdateSmallBlind();
            return;
        }
        if(playerOrderIndex == -1)
        {
            SetPlayerOrder();
            state = States.DealNextCard;
            return;
        }
        SendRoundInfo(Players[PlayerOrder[playerOrderIndex]].Player, bet, pot, round);
        state = States.WaitForPlayer;
    }
    
    /// <summary>
    /// Use hand regocnition to declare a winner, move big and small blind, increase round count, set state back to GameLoop
    /// </summary>
    public void showDown()
    {
        //evaluate hands and declare a winner for the round, maybe use an event for this
        GameObject winner = compareHands();
        Debug.Log("winner" + winner.GetComponent<PlayerControllerPoker>().PlayerName);
        ResetPlayersInRound();
        ResetPlayedCurrentRound();
        SetPlayerOrder();
        
        round = 1;
        NewShowDown();
        state = States.WaitForPlayer;
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
        foreach (PlayerContainer player in Players)
        {
            if (player.InRound.Equals(true))
            {
                player.HandRank = (handReco.HandRecognition(player.Player.GetComponent<PlayerControllerPoker>().hand));
                if (player.HandRank[0] > pokerHand)
                {

                    pokerHand = player.HandRank[0];
                }
            }
        }

        //find player(s) with the best poker hand
        foreach (PlayerContainer player in Players)
        {
            if (player.InRound)
            {
                Debug.Log("good");
                //////////////////////////////////Error
                if(player.HandRank[0] == pokerHand)
                {
                    Debug.Log("good");

                    pokerHandHigh.Add(player.Player);
                }
            }
        }

        //check if there is one player with the best hand
        //if not find the players with the best high card
        if (pokerHandHigh.Count != 1)
        {
            foreach (PlayerContainer player in Players)
            {
                if (player.InRound.Equals(true) && player.HandRank[0] == pokerHand)
                {
                    if (player.HandRank[1] > high)
                    {
                        high = player.HandRank[1];
                    }
                }
            }
            foreach (PlayerContainer player in Players)
            {
                if (player.InRound.Equals(true) && player.HandRank[0]==pokerHand)
                {
                    if (player.HandRank[1] == high)
                    {
                        highCard.Add(player.Player);
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
            foreach (PlayerContainer player in Players)
            {
                if (player.InRound.Equals(true) && player.HandRank[0] == pokerHand && player.HandRank[1] == high)
                {
                    if (player.HandRank[2] > suit)
                    {
                        suit = player.HandRank[2];
                    }
                }
            }
            foreach (PlayerContainer player in Players)
            {
                if (player.InRound.Equals(true) && player.HandRank[0] == pokerHand && player.HandRank[1] == high)
                {
                    if (player.HandRank[2] == suit)
                    {
                        suitHigh.Add(player.Player);
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
    public Card DealToPlayer(int numCards)
    {
        return deck.deal(numCards)[0];
    }

    /// <summary>
    /// Set inRound bool of all players still in the game to true
    /// </summary>
    public void ResetPlayersInRound()
    {
        foreach(PlayerContainer x in Players)
        {
            if (x.InGame)
            {
                x.InRound = true;
            }
        }
    }


    /// <summary>
    /// Remove @param player form the current round
    /// </summary>
    /// <param name="player"></param>
    public void RemovePlayerFromRound(PlayerControllerPoker player)
    {
        Players.First(x => player).InRound = false;
    }

    /// <summary>
    /// Removes the player from the game
    /// </summary>
    /// <param name="player"></param>
    public void RemovePlayerFromGame(GameObject player)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].Player == player)
            {
                Players[i].InGame =false;
            }
        }
    }

    /// <summary>
    /// Reset the playedcurrentround bool for players in round
    /// </summary>
    public void ResetPlayedCurrentRound()
    {
        foreach(PlayerContainer x in Players)
        {
            if (x.InRound)
            {
                x.PlayedCurrentRound = false;
            }
        }
    }
    
    /// <summary>
    /// Reset the play order list after raise
    /// </summary>
    /// <param name="player"></param>
    public void ResetPlayOrder()
    { 
        int j = PlayerOrder[playerOrderIndex];
        PlayerOrder.Clear();
        for(int i = j+1; i < Players.Count; i++)
        {
            if (Players[i].InRound)
            {
                PlayerOrder.Add(i);
            }
        }
        for(int i = 0; i < j; i++)
        {
            if (Players[i].InRound)
            {
                PlayerOrder.Add(i);
            }
        }
        playerOrderIndex = 0;
        foreach(int x in PlayerOrder)
        {
            Debug.Log(x);
        }
    }

    /// <summary>
    /// Increments the playerOrder int, if at the end of the player order increment round
    /// </summary>
    public void IncrementPlayerOrder()
    {
         playerOrderIndex++;
        if(playerOrderIndex >= PlayerOrder.Count)
        {
            playerOrderIndex = -1;
            round++;
        }
    }

    /// <summary>
    /// Check if there is one player in the current round
    /// </summary>
    public void CheckForShowdown()
    {
        int j = 0;
        foreach (PlayerContainer x in Players)
        {
            if (x.InRound)
            {
                j++;
            }
        }

        if (j == 1)
        {
            state = States.Showdown;
        }
    }

    /// <summary>
    /// Sets the playOrder list
    /// </summary>
    public void SetPlayerOrder()
    {
        PlayerOrder.Clear();
        int i = 0;
        foreach (PlayerContainer x in Players)
        {
            if (x.InRound == true)
            {
                PlayerOrder.Add(i);
            }
            i++;
        }
        playerOrderIndex = 0;
    }

    /// <summary>
    /// Update the big blind to the next player in the game
    /// </summary>
    /// <returns></returns>
    public void UpdateBigBlind()
    {
        for(int i = 0; i < Players.Count; i++)
        {
            if (Players[i].HasBeenBigBlind.Equals(false) && Players[i].InGame.Equals(true) && Players[i].InRound.Equals(true))
            {
                Players[i].HasBeenBigBlind = true;
                Players[i].IsBigBlind = (true);
                Debug.Log(Players[i].Player.GetComponent<PlayerControllerPoker>().PlayerName + " is big blind");
                Debug.Log(Players[i].IsBigBlind);
                Debug.Log(Players[i].HasBeenBigBlind);
                break;
            }
            Players[i].IsBigBlind = (false);
        }
    }

    /// <summary>
    /// Update the small blind
    /// </summary>
    public void UpdateSmallBlind()
    {
        foreach (PlayerContainer x in Players)
        {
            if (x.HasBeenSmallBlind.Equals(false) && x.InGame.Equals(true) && x.InRound.Equals(true))
            {
                x.HasBeenSmallBlind = (true);
                x.IsSmallBlind = (true);
                Debug.Log(x.Player.GetComponent<PlayerControllerPoker>().PlayerName + "is small blind");
                break;
            }
            x.IsSmallBlind = (false);
        }
    }
    

    /// <summary>
    /// Set small blind at the begining of the game
    /// </summary>
    public void SetSmallBlind()
    {
        Players[Players.Count - 1].IsSmallBlind = true;
        Players[Players.Count - 1].HasBeenSmallBlind = true;
    }

    /// <summary>
    /// Send an event to the big and small blind to take their initial bets
    /// </summary>
    public void TakeInitalBets()
    {
        foreach(PlayerContainer x in Players)
        {
            if (x.InRound && x.IsBigBlind && x.Currency - bigBlindAmt > 0)
            {
                pot += bigBlindAmt;
                x.Currency = (x.Currency - bigBlindAmt);
                TakeBigBet(x.Player, bigBlindAmt);
            }
            if (x.InRound && x.IsSmallBlind && x.Currency - smallBlindAmt > 0)
            {
                pot += smallBlindAmt;
                x.Currency = (x.Currency - smallBlindAmt);
                TakeSmallBet(x.Player, smallBlindAmt);
            }
        }
    }

    /// <summary>
    /// Checks if every player has been the big blind
    /// </summary>
    /// <returns></returns>
    public bool CheckAllPlayersBigBlind()
    {
        foreach(PlayerContainer x in Players)
        {
            if (x.HasBeenBigBlind.Equals(false))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Send event to players that there there is going to be a new hand
    /// </summary>
    public void NewHand()
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

    public class NewHandEventArgs
    {


        public NewHandEventArgs()
        {
        }
    }

    public static event System.EventHandler<NewHandEventArgs> NewHandEvent;

    public void ClearHand()
    {
        NewHandEvent?.Invoke(this, new NewHandEventArgs());
    }

    /* EVENT 6
    * Tell players that showdown is happening
    */

    public class ShowDownEventArgs
    {

        public ShowDownEventArgs()
        {

        }
    }

    public static event System.EventHandler<ShowDownEventArgs> ShowDownEvents;

    public void NewShowDown()
    {
        ShowDownEvents?.Invoke(this, new ShowDownEventArgs());
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

    private void Player_RoundInfo(object sender, PlayerControllerPoker.RoundDecisionEventArgs args)
    {
        if (args.fold)
        {
            //remove player from queue
            RemovePlayerFromRound(args.thisPlayer);
            IncrementPlayerOrder();
        }
        else if (args.raise)
        {
            //player raised
            bet = args.bet;
            pot += args.bet;
            ResetPlayOrder();

        }
        else //Player checked the bet
        {
            pot += args.bet;
            IncrementPlayerOrder();
        }
        state = States.PromptPlayer;
    }

    /* EVENT HANDLE    
   * Showdown Response
   * continue to the next hand
   */

    private void Player_ShowDownResponse(object sender, PlayerControllerPoker.ShowDownResponseEventArgs args)
    {
        NewHand();
        pot = 0;
        bet = 0;
        state = States.Deal;

    }

    /* EVENT HANDLE
   * Major Arcana Effects
   * receive effects: force player to fold for a round, deal cards to a player, force a bet increase
   * There will be others
   * May have to be separate events
   */

    #endregion Events
}