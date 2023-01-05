using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public string playerName;

    public int playerBronze;
    public int playerSilver;
    public int playerGold;
    public TextMeshProUGUI bronzeText;
    public TextMeshProUGUI silverText;
    public TextMeshProUGUI goldText;

    private int bronze = 1;
    private int silver = 10;
    private int gold = 100;

    public bool bronzeTracker = true;
    public bool silverTracker = true;
    public bool goldTracker = true;

    private void Start()
    {
        playerBronze = 30;
        playerSilver = 10;
        playerGold = 3;

        bronzeText.text = "Bronze:" + playerBronze.ToString();
        silverText.text = "Silver:" + playerSilver.ToString();
        goldText.text = "Gold:" + playerGold.ToString();
   
    }
    public void addBronze(int bronzeToAdd)
    {
        playerBronze += bronzeToAdd;
        bronzeText.text = "Bronze:" + playerBronze.ToString();
    }

    public void addSilver(int silverToAdd)
    {
        playerSilver += silverToAdd;
        silverText.text = "Silver:" + playerSilver.ToString();
    }

    public void addGold(int goldToAdd)
    {
        playerGold += goldToAdd;
        goldText.text = "Gold:" + playerGold.ToString();
    }

    public void subtractBronze(int bronzeToSubrtact)
    {
        if(playerBronze - bronzeToSubrtact < 0)
        {
            Debug.Log("Out of bronze!");
        }
        else
        {
            playerBronze -= bronzeToSubrtact;
            bronzeText.text = "Bronze:" + playerBronze.ToString();
        }
        
    }

    public void subtractSilver(int silverToSubrtact)
    {
        if (playerSilver - silverToSubrtact < 0)
        {
            Debug.Log("Out of silver!");
        }
        else
        {
            playerSilver -= silverToSubrtact;
            silverText.text = "Silver:" + playerSilver.ToString();
        }

    }

    public void subtractGold(int goldToSubrtact)
    {
        if (playerGold - goldToSubrtact < 0)
        {
            Debug.Log("Out of gold!");
        }
        else
        {
            playerGold -= goldToSubrtact;
            goldText.text = "Gold:" + playerGold.ToString();
        }

    }
}
