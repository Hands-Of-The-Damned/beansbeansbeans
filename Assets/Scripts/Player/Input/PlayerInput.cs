//Mickey Kerr 2023
//Original playercontroller code provided by Jarod Gobert.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : ActorInput
{
    //should the player choose to not confirm in the options menu, they will simply not need to confirm.
    #region Options
    ///Allows the player to not confirm.
    [Header("noConfirm Options")]
    public bool noConfirmAll = false;
    public bool noConfirmFold = false;
    public bool noConfirmEndTurn = false;
    public bool noConfirmPlayCard = false;
    #endregion


    #region Unity Events
    [Header("Events")]
    //for animations and such, basically to make other people's lives a little easier.
    //i.e. particle effect plays when a player folds on event
    public UnityEvent betFailEvent;
    public UnityEvent foldEvent;
    public UnityEvent addToBetEvent;
    public UnityEvent negToBetEvent;
    public UnityEvent checkEvent;
    public UnityEvent showdownEvent;
    public UnityEvent endTurnEvent;
    public UnityEvent endTurnFailEvent;
    public UnityEvent playCardEvent;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(noConfirmAll)
        {
            noConfirmPlayCard = noConfirmFold = noConfirmEndTurn = true;
        }
        actorController = gameObject.GetComponent<PlayerControllerPoker>();
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerInput sends information to player controller on what needs to be done.
        
        //can only play game if it is turn
        if (actorController.isTurn)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //may be a good idea to prompt player
                actorFold();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                addToBet(1);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                addToBet(-1);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Check();
            }

            //"Return" is the enter key that is NOT on the keypad
            if (Input.GetKeyDown(KeyCode.Return))
            {
                endTurn();
            }
        }
    }

    //The variables below are made to be public to allow them to be tied to buttons on the UI for the player to use, should they not wish to use the keyboard.
    #region publicVariables
    public override void actorFold()
    {
        Debug.Log(actorController.playerName + " folded!");
        //prompt player to press again to confirm
        
        if (confirmFold())
        {
            foldEvent.Invoke();
            actorController.flipFold();
        }
    }

    //player must confirm if they wish to fold, end their turn, or play a card.
    public bool confirmFold()
    {
        bool isConfirm = false;

        if (noConfirmFold)
        {
            isConfirm = true;
        }
        /*
         * Placeholder area for subroutine to confirm folding
         */
        return isConfirm;
    }

    //allows player to add/take away money from their bet.
    public override void addToBet(int numberAddedToBet)
    {
        Debug.Log("Adding " + numberAddedToBet.ToString() + " to bet!");
        int currentMoney = actorController.currency;
        int currentBet = actorController.bet;
        int totalbet;
        if(numberAddedToBet > 0)
        {
            addToBetEvent.Invoke();
        }

        if(numberAddedToBet < 0)
        {
            negToBetEvent.Invoke();
        }

        //enough money for bet?
        if (currentMoney - numberAddedToBet < 0)
        {
            //not enough money...
            //event to display to player that they are broke, lmao
            betFailEvent.Invoke();
            //kick them out of function for having no money,
            //kinda like how I'll get kicked out of university for missing a payment
            return;
        }

        totalbet = currentBet + numberAddedToBet;
        actorController.setBet(numberAddedToBet + currentBet);
    }

    //ends turn.
    public override void endTurn()
    {
        Debug.Log("Ending " + actorController.playerName + "'s turn!");
        //Logic to make sure that they are doing something legal.
        if(actorController.currentBetToMatch > actorController.bet && actorController.folded == false)
        {
            Debug.Log("Can't end turn, you have not folded, and your bet is lower than the bet to match.");
            endTurnFailEvent.Invoke();
        }
        //showdown turn
        if (actorController.isShowdown)
        {
            showdownEvent.Invoke();
            actorController.SendShowDownResponse();
        }

        //normal turn
        if (actorController.isTurn) 
        { 
            endTurnEvent.Invoke();
            actorController.playHand(); 
        }

    }

    //sets bet to currentbettoMatch
    public override void Check()
    {
        checkEvent.Invoke();
        Debug.Log("Checking to bet!");
        actorController.setBet(actorController.currentBetToMatch);
    }

    // plays card. May be better as subroutine
    public override void playCard()
    {
        playCardEvent.Invoke();
        Debug.Log("Playing a Major Arcana Card!");

        //add code when MajorArcana are implemented

    }

    #endregion
}
