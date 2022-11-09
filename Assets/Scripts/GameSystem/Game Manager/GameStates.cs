
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameStates : MonoBehaviour
{
    
    GameObject Player;

    enum states
    {
        Initial,
        GameLoop,
        Showdown,
        EndGame
    }

    GameManagerPublisher eventSystem;
    states state = states.Initial;
    Player[] players;
    Player[] playersInRound;
    Queue playerQueue;
    Deck deck;
    bool allPlayersBigBlind = false;
    bool showDownBool;
    public bool playerReply;
    int bigBlind;
    int smallBlind;
    int round;
    public int pot;
    public int bet;
    int smallBlindAmt;
    int bigBlindAmt;

    void Start()
    {
        bigBlind = 0;
        smallBlind = players.Length-1;
        showDownBool = false;
        playerReply = false;
        round = 1;
        pot = 0;
        bet = 0;
        smallBlindAmt = 1;
        bigBlindAmt = 2;
    }

    void Update()
    {
        switch (state)
        {
            case states.Initial:
                gameInitialize(players);
                break;

            case states.GameLoop:
                gameLoop();
                break;

            case states.EndGame:
                endGame();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// This function will take a player array full of the players in the game.
    /// The array is used to populate the player queue for the gameloop
    /// Initilizes game objects
    /// </summary>
    /// <param name="players"></param>
    //might need to change return type
    public void gameInitialize(Player[] playersInGame)
    {
        playerQueue = new Queue();
        players = playersInGame;
        //Initialize game objects
        deck = new Deck();
        //add more relevent game objects

        state = states.GameLoop;
    }


    public void gameLoop()
    {
        while (!allPlayersBigBlind)
        {
            //Play the game 
            deck.shuffle();
            setPlayersInRound();
            takeInitalBets();

            //Deal the first 5 cards
            initialDeal();
            while (!showDownBool)
            {
                setQueue();
                foreach (Player x in playerQueue)
                {
                    //send event into void declaring which players turn it is
                    eventSystem.SendRoundInfo(x, bet, pot, round);
                    //wait for reply
                    while (!playerReply) { }
                    playerReply = false;

                }

                //check if there is one player left in the round
                if (onePlayerInRound()){
                    showDownBool = true;
                }
                //Check if we delt 8 cards to all players
                if(round > 3)
                {
                    showDownBool = true;
                }
                round++;
                //Once players play their hand deal all players 1 card
                foreach(Player x in playersInRound)
                {
                    if(x != null)
                    {
                        eventSystem.DealToPlayer(x, dealToPlayer(1, x));
                    }
                }
            }
            //Do showdown
            showDown();
            allPlayersBigBlind = updateBigBlind();
            round = 0;
           
        }
        state = states.EndGame;
    }

    /// <summary>
    /// Use hand regocnition to declare a winner, move big and small blind, increase round count, set state back to GameLoop
    /// </summary>
    public void showDown()
    {
        //evaluate hands and declare a winner for the round, maybe use an event for this
        showDownBool = false;
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
        for(int i = 0; i < playersInRound.Length; i++)
        {
            if(player == playersInRound[i])
            {
                playersInRound[i] = null;
            }
        }
    }

    /// <summary>
    /// Removes the player from the game
    /// </summary>
    /// <param name="player"></param>
    public void removePlayerFromGame(Player player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (player == players[i])
            {
                players[i] = null;
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
            foreach (Player x in playersInRound)
            {
                if(x != null)
                {
                    eventSystem.DealToPlayer(x, dealToPlayer(1, x));
                }
            }
        }
    }


    /// <summary>
    /// Reset the queue with @param player at the end of queue
    /// </summary>
    /// <param name="player"></param>
    public void resetQueue(Player player)
    {
        int j = 0;
        for (int i = 0; i < playersInRound.Length; i++)
        {
            if (player == playersInRound[i])
            {
                j = i;
            }
        }

        foreach(Player x in playerQueue)
        {
            playerQueue.Enqueue(x);
        }

        for (int i = j; i < playersInRound.Length; i++)
        {
            if (playersInRound[i] != null)
            {
                playerQueue.Enqueue(playersInRound[i]);
            }
        }
    }

    /// <summary>
    /// Set the queue with the current players in the round
    /// </summary>
    public void setQueue()
    {
        foreach (Player x in playersInRound)
        {
            if (x != null)
            {
                playerQueue.Enqueue(x);
            }
        }
    }

    /// <summary>
    /// Set the playersInRound array with the players that are in the players array
    /// </summary>
    public void setPlayersInRound()
    {
        
            for(int i = 0; i < players.Length; i++)
            {
            if (players[i] != null)
            {
                playersInRound[i] = players[i];
                }
            }
        
    }

    /// <summary>
    /// Update the big blind to the next player in the game
    /// </summary>
    /// <returns></returns>
    public bool updateBigBlind()
    {
        bigBlind++;
        if(bigBlind > players.Length-1)
        {
            return true;
        }
        if(players[bigBlind] == null && bigBlind < players.Length-1)
        {
            updateBigBlind();
        }
        updateSmallBlind(bigBlind-1);
        return false;
    }

    /// <summary>
    /// Update the small blind based on current big blind
    /// </summary>
    /// <param name="newSmallBlind"></param>
    public void updateSmallBlind(int newSmallBlind)
    {
        smallBlind = newSmallBlind;
        if (players[smallBlind] == null && smallBlind >= 0)
        {
            updateSmallBlind(smallBlind - 1);
        }
        else
        {
            smallBlind = -1;
        }
    }

    /// <summary>
    /// Check if there is one player left in the round
    /// </summary>
    /// <returns></returns>
    public bool onePlayerInRound()
    {
        int i = 0;
        foreach(Player x in playersInRound)
        {
            if(x != null)
            {
                i++;
            }
        }
        if (i > 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Send an event to the big and small blind to take their initial bets
    /// </summary>
    public void takeInitalBets()
    {
        eventSystem.TakeBigBet(playersInRound[bigBlind], bigBlindAmt);
        pot += bigBlindAmt;

        if(playersInRound[bigBlind] != null)
        {
            eventSystem.TakeSmallBet(playersInRound[smallBlind], smallBlindAmt);
            pot += smallBlindAmt;
        }
        
    }
}


