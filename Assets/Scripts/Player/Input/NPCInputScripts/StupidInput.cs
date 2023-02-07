//Mickey Kerr 2023

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

/*
 * Name of the game for this AI script is that the AI is dumb, and is only able to make decisions
 * based off of dice and coin flips. Think your friend who brings dice and coins everywhere,
 * and INSISTS on using them whenever possible, even if it's to their own detriment.
 * 
 * So basically, program me as an AI when I play poker. 
 * 
 * 
 * I am spectacularly bad at poker.
 */
public class StupidInput : ActorInput
{
    #region Private Variables
    private bool _turnHasStarted = false;
    #endregion

    #region Unity Events
    [Header("Events")]
    //for animations and such, basically to make other people's lives a little easier.
    //i.e. particle effect plays when a player folds on event
    public UnityEvent foldEvent;
    public UnityEvent addToBetEvent;
    public UnityEvent negToBetEvent;
    public UnityEvent checkEvent;
    public UnityEvent showdownEvent;
    public UnityEvent endTurnEvent;
    public UnityEvent playCardEvent;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        actorController = gameObject.GetComponent<PlayerControllerPoker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (actorController.isTurn && (_turnHasStarted == false))
        {
            _turnHasStarted = true;
            StartCoroutine(TurnActions());
        }
    }

    /// <summary>
    /// Flips a coin.
    /// </summary>
    /// <returns>Returns a random boolean of true or false.</returns>
    private bool CoinFlip()
    {
        //generates integer value that is either 0 or 1. Don't ask
        //why the unity devs made it behave this way for integers,
        //it doesn't behave this way for floats
        int coin = Random.Range(0, 2);

        //if tails
        if (coin == 0) { return false; }

        //if heads
        else { return true; }

    }

    private int D20Roll()
    {
        return Random.Range(0, 21);

    }

    //needs to be a coroutine so that waitForSeconds doesn't cause the computer
    //to freeze. Which is generally considered bad UX, so I've heard
    IEnumerator TurnActions()
    {
        //player turn starts
        //fold or check?
        //fold condition
        bool coin = CoinFlip();
        bool finishTurn = true;
        yield return new WaitForSeconds(2f);
        //fold, and end turn, then exit coroutine
        if (coin == false)
        {
            actorFold();
            endTurn();
            yield break;
        }
        else
        {
            Check();
        }

        yield return new WaitForSeconds(2f);
        //flip coin again to raise bet or to play a card.
        coin = CoinFlip();
        if (coin)
        {
            //roll a dice to figure out how much to bet extra.
            addToBet(D20Roll());
        }
        else
        {
            playCard();
        }

        while (finishTurn)
        {
            yield return new WaitForSeconds(2f);
            coin = CoinFlip();

            if (coin)
            {
                addToBet(D20Roll());
            }
            else
            {
                endTurn();
                finishTurn = false;
            }
        }
        //flip coin again to raise the bet or to play a card
        
    }
}
