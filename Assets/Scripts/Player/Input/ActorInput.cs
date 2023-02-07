//Mickey Kerr 2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActorInput : MonoBehaviour
{
    #region PublicGlobalVars

    public PlayerControllerPoker actorController;
    public playerHand playerHand;

    #endregion

    // Start is called before the first frame update. Will need to get actorController var.
    private void Start()
    {
        actorController = gameObject.GetComponent<PlayerControllerPoker>();
    }

    //Just fold. If you wish to end turn, this must be specified
    public virtual void actorFold()
    {
        actorController.setFold(true);
    }

    //allows player to add/take away money from their bet.
    public virtual void addToBet(int numberAddedToBet)
    {
        Debug.Log("Adding " + numberAddedToBet.ToString() + " to bet!");
        int currentMoney = actorController.currency;

        //enough money for bet?
        if (currentMoney + numberAddedToBet < 0)
        {
            //not enough money...
            //kick them out of function for having no money,
            //kinda like how I'll get kicked out of university for missing a payment
            //:(
            return;
        }

        actorController.setBet(numberAddedToBet);
    }

    //ends turn.
    public virtual void endTurn()
    {
        Debug.Log("Ending " + actorController.playerName + "'s turn!");
        //showdown turn
        if (actorController.isShowdown)
        {
            actorController.SendShowDownResponse();
        }

        //normal turn
        if (actorController.isTurn) { actorController.playHand(); }

    }

    //sets bet to currentbettoMatch
    public virtual void Check()
    {
        Debug.Log("Checking to bet!");
        actorController.setBet(actorController.currentBetToMatch);
    }

    // plays card. May be better as subroutine
    public virtual void playCard()
    {
        Debug.Log("Playing a Major Arcana Card!");

        //add code when MajorArcana are implemented

    }
}
