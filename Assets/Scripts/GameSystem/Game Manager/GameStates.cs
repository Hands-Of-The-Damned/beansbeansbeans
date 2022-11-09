
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
    int bigBlind;
    int smallBlind;
    int round;

    void Start()
    {
        bigBlind = 0;
        smallBlind = players.Length-1;
        showDownBool = false;
        round = 1;
    }

    void Update()
    {
        switch (state)
        {
            case states.Initial:
                gameInitialize(players);
                break;

            case states.GameLoop:
                gameLoop(playerQueue, deck);
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
    void gameInitialize(Player[] players)
    {
        playerQueue = new Queue();

        //Initialize game objects
        Deck deck = new Deck();
        //add more relevent game objects

        state = states.GameLoop;
    }


    void gameLoop(Queue players, Deck deck)
    {
        while (!allPlayersBigBlind)
        {
            //Play the game 
            deck.shuffle();
            setPlayersInRound();
            setQueue();
            //take bet from big and small blind

            //Deal the first 5 cards
            initialDeal();
            while (!showDownBool)
            {
                foreach (Player x in playerQueue)
                {
                    //send event into void declaring which players turn it is
                    //wait for player to send the event with decision info
                    //do something with the info packet

                }

                if (onePlayerInRound()){
                    showDownBool = true;
                }

                if(round > 3)
                {
                    showDownBool = true;
                }
                round++;
                //deal all players 1 card
                foreach(Player x in playersInRound)
                {
                    if(x != null)
                    {
                        dealToPlayer(1, x);
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
    void showDown()
    {
        //evaluate hands and declare a winner for the round, maybe use an event for this
        showDownBool = false;
    }

    /// <summary>
    /// Display the end of game screen
    /// </summary>
    void endGame()
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
    Card dealToPlayer(int numCards, Player player)
    {
        return deck.deal(numCards)[0];
    }

    /// <summary>
    /// Remove @param player form the current round
    /// </summary>
    /// <param name="player"></param>
    void removePlayerFormRound(Player player)
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
    void removePlayerFromGame(Player player)
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
    void initialDeal()
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
    void resetQueue(Player player)
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
    void setQueue()
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
    void setPlayersInRound()
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
    bool updateBigBlind()
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
    void updateSmallBlind(int newSmallBlind)
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
    bool onePlayerInRound()
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
}


