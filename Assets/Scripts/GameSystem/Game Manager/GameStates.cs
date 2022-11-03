using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : MonoBehaviour
{
    // Start is called before the first frame update

    enum states
    {
        Initial,
        GameLoop,
        EndGame
    }

    GameManagerPublisher eventSystem;
    states state = states.Initial;  
    Player[] players;
    Player[] playersInRound;
    Queue playerQueue;
    Deck deck;
    bool raise = false;
    int handcount;

    void Start()
    {
        handcount = 1;
    }

    // Update is called once per frame
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
    /// </summary>
    /// <param name="players"></param>
    //might need to change return type
    void gameInitialize(Player[] players)
    {
        Queue PlayerQueue = new Queue();
        playersInRound = players;
        foreach(Player x in players)
        {
            //populate player queue
        }

        //Initialize game objects
        Deck deck = new Deck();
        //add more relevent game objects

        state = states.GameLoop;
    }


    void gameLoop(Queue players, Deck deck)
    {
        while(everyone not yet big blind){
            while (not showdown)
            {
                //Play the game 
                deck.shuffle();
                //check if player is ready or wants to exit game

                //Deal the first 5 cards
                initialDeal(playersInRound);
                

                foreach (Player x in playerQueue)
                {
                    //send event into void declaring which players turn it is
                    //wait for player to send the event with decision info
                    //do something with the info packet

                }


                if (1 player still in)
                    end round
            }

        }
        state = states.EndGame;
    }


    void endGame()
    {
        /*
         * Exit the game(return to menu or open world)
         * maybe some other stuff
         */
    }

    string dealToPlayer(int numCards, Player player)
    {
        return deck.deal(numCards);
    }

    void removePlayerFormRound(PlayerPrefs player)
    {
        //remove player from playerinround array
    }

    void removePlayerFromGame(Player player)
    {
        //remove player from players array
    }

    //Deal the first five cards to the players 1 at a time
    void initialDeal(Player[] players)
    {
        for(int i = 0; i < players.Length; i++)
        {
            foreach(Player x in players)
            {
                eventSystem.DealToPlayer(x, dealToPlayer(1,x));
            }
        }
    }


    //Turn this into an event!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    Queue playerRaised(Queue players)
    {
        //If one player raised, reset the queue to hit all other players

        return players;
    }
}

