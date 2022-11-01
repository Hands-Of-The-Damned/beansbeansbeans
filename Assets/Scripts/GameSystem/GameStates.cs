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

    states state = states.Initial;  
    Player[] = players;
    Queue playerQueue;
    Deck deck;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case states.Initial:
                GameInitialize(players);
                break;

            case states.GameLoop:
                GameLoop(playerQueue, deck);
                break;

            case states.EndGame:
                //call endgame function
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
    void GameInitialize(Player[] players)
    {
        Queue PlayerQueue = new Queue();

        foreach(Player in players)
        {
            //populate player queue
        }

        //Initialize game objects
        Deck deck = new Deck();
        //add more relevent game objects

        state = states.GameLoop;
    }


    void GameLoop(Queue players, Deck deck)
    {
        while(state == states.GameLoop)
        {
            //Play the game 

            if(all players are out)
                state = states.EndGame;
        }
    }

    void Endgame()
    {
        /*
         * Exit the game(return to menu or open world)
         * maybe some other stuff
         */
    }



}

